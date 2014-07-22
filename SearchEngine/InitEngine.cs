using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;



namespace SearchEngine
{
    public class SearchEngineLucene:IDisposable
    {
        private readonly Directory _directory;
        private Analyzer analyzer;
        private List<Document> documentSet;
        private Document _singleDocument;
        private readonly IndexWriter writer;

        public SearchEngineLucene(bool create)
        {
            documentSet = new List<Document>();
            _directory = FSDirectory.Open(new DirectoryInfo(Environment.CurrentDirectory + "\\LuceneIndex"));
            analyzer = new StandardAnalyzer(Version.LUCENE_30);
            writer = new IndexWriter(_directory, analyzer, create, IndexWriter.MaxFieldLength.LIMITED);
          
            
        }
        public void CreateNewDocument()
        {
            _singleDocument = new Document();

        }

        public void AddField(string name, string value,string _storeType,string _indexType )
        {
            var storeType = (Field.Store)Enum.Parse(typeof(Field.Store), _storeType);
            var indexType = (Field.Index)Enum.Parse(typeof(Field.Index), _indexType);
            _singleDocument.Add(new Field(name, value, storeType, indexType));
        }
        public void AddDocument()
        {
            writer.AddDocument(_singleDocument);
            writer.Optimize();           
        }
        public List<string> Search(string field, string needle)
        {
          if(writer != null)  writer.Dispose();
            var results = new List<string>();
            IndexReader indexReader = IndexReader.Open(_directory, true);
            Searcher indexSearch = new IndexSearcher(indexReader);
            var queryParser = new QueryParser(Version.LUCENE_30, field, analyzer);
            var query = queryParser.Parse(needle);           
            TopDocs resultDocs = indexSearch.Search(query, indexReader.MaxDoc);          
            var hits = resultDocs.ScoreDocs;           
            foreach (var hit in hits)
            {            
               var documentFromSearcher = indexSearch.Doc(hit.Doc);
               results.Add(documentFromSearcher.Get(field));
            }
            indexReader.Dispose();
            indexSearch.Dispose();
            return results;        
        }


        public List<string> Search(string field, string needle,string fieldShown)
        {
            if (writer != null) writer.Dispose();
            List<string> results = new List<string>();
            IndexReader indexReader = IndexReader.Open(_directory, true);
            Searcher indexSearch = new IndexSearcher(indexReader);
            var queryParser = new QueryParser(Version.LUCENE_30, field, analyzer);
            var query = queryParser.Parse(needle);
            TopDocs resultDocs = indexSearch.Search(query, indexReader.MaxDoc);
            var hits = resultDocs.ScoreDocs;
            foreach (var hit in hits)
            {
                var documentFromSearcher = indexSearch.Doc(hit.Doc);
                results.Add(documentFromSearcher.Get(fieldShown));
            }
            indexReader.Dispose();
            indexSearch.Dispose();
            return results;
        }











        public void Dispose()
        {
             Dispose(true);
             GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
             if (disposing)
             {
                 if (writer != null) writer.Dispose();            
             }        
        }

    }
}


