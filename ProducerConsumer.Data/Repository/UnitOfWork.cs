using ProducerConsumer.Data.Entities;
using ProducerConsumer.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProducerConsumerDbContext DbContext;

        public IGenericRepository<MessageList> _messageRepository { get; set; }

        public UnitOfWork(ProducerConsumerDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IGenericRepository<MessageList> MessageRepository
        {
            get
            {
                if(_messageRepository == null)
                {
                    _messageRepository = new GenericRepository<MessageList>(DbContext);
                }
                return _messageRepository;
            }
        }

        public bool Commit()
        {
            return DbContext.SaveChanges() > 0;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
