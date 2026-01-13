using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestAPI.Models;

namespace TestAPI.Services
{
    public interface IBooksService
    {
        Task<IEnumerable<Book>> GetAsync();
        Task<Book?> GetAsync(string id);
        Task CreateAsync(Book newBook);
        Task UpdateAsync(string id, Book updatedBook);
        Task RemoveAsync(string id);
    }

    public class BooksService : IBooksService
    {
        private readonly IMongoCollection<Book> booksCollection;

        public BooksService(IOptions<BookStoreDatabaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.DatabaseName);
            booksCollection = db.GetCollection<Book>(options.Value.BooksCollectionName);
        }

        public async Task CreateAsync(Book newBook)
        {
            await booksCollection.InsertOneAsync(newBook);
        }

        public async Task<IEnumerable<Book>> GetAsync()
        {
            return await booksCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Book?> GetAsync(string id)
        {
            return await booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await booksCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(string id, Book updatedBook)
        {
            await booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);
        }
    }
}