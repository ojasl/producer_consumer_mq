using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProducerConsumer.Data.Entities;

[Table("MessageList")]
public partial class MessageList
{
    [Key]
    public int Id { get; set; }

    public string Message { get; set; } = null!;

    [StringLength(50)]
    public string Status { get; set; } = null!;
}
