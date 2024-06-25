using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProducerConsumer.Data.Entities;

public partial class ProducerConsumerDbContext : DbContext
{
    public ProducerConsumerDbContext()
    {
    }

    public ProducerConsumerDbContext(DbContextOptions<ProducerConsumerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MessageList> MessageLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Messages");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
