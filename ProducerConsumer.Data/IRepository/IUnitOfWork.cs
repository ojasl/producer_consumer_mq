using ProducerConsumer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Data.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<MessageList> MessageRepository { get; }

        bool Commit();
    }
}
