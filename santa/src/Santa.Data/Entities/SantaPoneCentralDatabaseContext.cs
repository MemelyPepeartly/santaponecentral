using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Santa.Data.Entities
{
    public partial class SantaPoneCentralDatabaseContext : DbContext
    {
        public SantaPoneCentralDatabaseContext()
        {
        }

        public SantaPoneCentralDatabaseContext(DbContextOptions<SantaPoneCentralDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientRelationXref> ClientRelationXref { get; set; }
        public virtual DbSet<ClientStatus> ClientStatus { get; set; }
        public virtual DbSet<ClientTagXref> ClientTagXref { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<SurveyOption> SurveyOption { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public virtual DbSet<SurveyQuestionOptionXref> SurveyQuestionOptionXref { get; set; }
        public virtual DbSet<SurveyQuestionXref> SurveyQuestionXref { get; set; }
        public virtual DbSet<SurveyResponse> SurveyResponse { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessage", "app");

                entity.Property(e => e.ChatMessageId)
                    .HasColumnName("chatMessageID")
                    .HasViewColumnName("chatMessageID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientRelationXrefId)
                    .HasColumnName("clientRelationXrefID")
                    .HasViewColumnName("clientRelationXrefID");

                entity.Property(e => e.DateTimeSent)
                    .HasColumnName("dateTimeSent")
                    .HasViewColumnName("dateTimeSent")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsMessageRead)
                    .HasColumnName("isMessageRead")
                    .HasViewColumnName("isMessageRead");

                entity.Property(e => e.MessageContent)
                    .IsRequired()
                    .HasColumnName("messageContent")
                    .HasViewColumnName("messageContent")
                    .HasMaxLength(500);

                entity.Property(e => e.MessageRecieverClientId)
                    .HasColumnName("messageRecieverClientID")
                    .HasViewColumnName("messageRecieverClientID");

                entity.Property(e => e.MessageSenderClientId)
                    .HasColumnName("messageSenderClientID")
                    .HasViewColumnName("messageSenderClientID");

                entity.HasOne(d => d.ClientRelationXref)
                    .WithMany(p => p.ChatMessage)
                    .HasForeignKey(x => x.ClientRelationXrefId)
                    .HasConstraintName("FK__ChatMessa__clien__151E51AA");

                entity.HasOne(d => d.MessageRecieverClient)
                    .WithMany(p => p.ChatMessageMessageRecieverClient)
                    .HasForeignKey(x => x.MessageRecieverClientId)
                    .HasConstraintName("FK__ChatMessa__messa__142A2D71");

                entity.HasOne(d => d.MessageSenderClient)
                    .WithMany(p => p.ChatMessageMessageSenderClient)
                    .HasForeignKey(x => x.MessageSenderClientId)
                    .HasConstraintName("FK__ChatMessa__messa__13360938");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "app");

                entity.HasIndex(x => x.Email)
                    .HasName("UQ__Client__AB6E6164A4169E54")
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientID")
                    .HasViewColumnName("clientID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasColumnName("addressLine1")
                    .HasViewColumnName("addressLine1")
                    .HasMaxLength(50);

                entity.Property(e => e.AddressLine2)
                    .HasColumnName("addressLine2")
                    .HasViewColumnName("addressLine2")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasViewColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasColumnName("clientName")
                    .HasViewColumnName("clientName")
                    .HasMaxLength(50);

                entity.Property(e => e.ClientStatusId)
                    .HasColumnName("clientStatusID")
                    .HasViewColumnName("clientStatusID");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasViewColumnName("country")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasViewColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Nickname)
                    .HasColumnName("nickname")
                    .HasViewColumnName("nickname")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnName("postalCode")
                    .HasViewColumnName("postalCode")
                    .HasMaxLength(25);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasViewColumnName("state")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ClientStatus)
                    .WithMany(p => p.Client)
                    .HasForeignKey(x => x.ClientStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__clientSt__6C1C3C17");
            });

            modelBuilder.Entity<ClientRelationXref>(entity =>
            {
                entity.ToTable("ClientRelationXref", "app");

                entity.HasIndex(x => new { x.SenderClientId, x.RecipientClientId, x.EventTypeId })
                    .HasName("clientRelationXrefID")
                    .IsUnique();

                entity.Property(e => e.ClientRelationXrefId)
                    .HasColumnName("clientRelationXrefID")
                    .HasViewColumnName("clientRelationXrefID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("eventTypeID")
                    .HasViewColumnName("eventTypeID");

                entity.Property(e => e.RecipientClientId)
                    .HasColumnName("recipientClientID")
                    .HasViewColumnName("recipientClientID");

                entity.Property(e => e.SenderClientId)
                    .HasColumnName("senderClientID")
                    .HasViewColumnName("senderClientID");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.ClientRelationXref)
                    .HasForeignKey(x => x.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__event__74B18218");

                entity.HasOne(d => d.RecipientClient)
                    .WithMany(p => p.ClientRelationXrefRecipientClient)
                    .HasForeignKey(x => x.RecipientClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__recip__73BD5DDF");

                entity.HasOne(d => d.SenderClient)
                    .WithMany(p => p.ClientRelationXrefSenderClient)
                    .HasForeignKey(x => x.SenderClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__sende__72C939A6");
            });

            modelBuilder.Entity<ClientStatus>(entity =>
            {
                entity.ToTable("ClientStatus", "app");

                entity.Property(e => e.ClientStatusId)
                    .HasColumnName("clientStatusID")
                    .HasViewColumnName("clientStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasColumnName("statusDescription")
                    .HasViewColumnName("statusDescription")
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<ClientTagXref>(entity =>
            {
                entity.ToTable("ClientTagXref", "app");

                entity.HasIndex(x => new { x.ClientId, x.TagId })
                    .HasName("clientTagXrefID")
                    .IsUnique();

                entity.Property(e => e.ClientTagXrefId)
                    .HasColumnName("clientTagXrefID")
                    .HasViewColumnName("clientTagXrefID");

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientID")
                    .HasViewColumnName("clientID");

                entity.Property(e => e.TagId)
                    .HasColumnName("tagID")
                    .HasViewColumnName("tagID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientTagXref)
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientTag__clien__0F657854");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ClientTagXref)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientTag__tagID__10599C8D");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("EventType", "app");

                entity.HasIndex(x => x.EventDescription)
                    .HasName("UQ__EventTyp__F516F46E8FD6AFC1")
                    .IsUnique();

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("eventTypeID")
                    .HasViewColumnName("eventTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventDescription)
                    .IsRequired()
                    .HasColumnName("eventDescription")
                    .HasViewColumnName("eventDescription")
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey", "app");

                entity.Property(e => e.SurveyId)
                    .HasColumnName("surveyID")
                    .HasViewColumnName("surveyID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("eventTypeID")
                    .HasViewColumnName("eventTypeID");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");

                entity.Property(e => e.SurveyDescription)
                    .IsRequired()
                    .HasColumnName("surveyDescription")
                    .HasViewColumnName("surveyDescription")
                    .HasMaxLength(100);

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Survey)
                    .HasForeignKey(x => x.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Survey__eventTyp__778DEEC3");
            });

            modelBuilder.Entity<SurveyOption>(entity =>
            {
                entity.ToTable("SurveyOption", "app");

                entity.Property(e => e.SurveyOptionId)
                    .HasColumnName("surveyOptionID")
                    .HasViewColumnName("surveyOptionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasColumnName("displayText")
                    .HasViewColumnName("displayText")
                    .HasMaxLength(100);

                entity.Property(e => e.SurveyOptionValue)
                    .IsRequired()
                    .HasColumnName("surveyOptionValue")
                    .HasViewColumnName("surveyOptionValue")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SurveyQuestion>(entity =>
            {
                entity.ToTable("SurveyQuestion", "app");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsSurveyOptionList)
                    .HasColumnName("isSurveyOptionList")
                    .HasViewColumnName("isSurveyOptionList");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasColumnName("questionText")
                    .HasViewColumnName("questionText")
                    .HasMaxLength(150);

                entity.Property(e => e.SenderCanView)
                    .HasColumnName("senderCanView")
                    .HasViewColumnName("senderCanView");
            });

            modelBuilder.Entity<SurveyQuestionOptionXref>(entity =>
            {
                entity.HasKey(x => x.SurveyQuestionOptionXref1)
                    .HasName("PK__SurveyQu__1BAA3BB10BCC137E");

                entity.ToTable("SurveyQuestionOptionXref", "app");

                entity.Property(e => e.SurveyQuestionOptionXref1)
                    .HasColumnName("surveyQuestionOptionXref")
                    .HasViewColumnName("surveyQuestionOptionXref");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");

                entity.Property(e => e.SortOrder)
                    .IsRequired()
                    .HasColumnName("sortOrder")
                    .HasViewColumnName("sortOrder")
                    .HasMaxLength(5);

                entity.Property(e => e.SurveyOptionId)
                    .HasColumnName("surveyOptionID")
                    .HasViewColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyQuestionOptionXref)
                    .HasForeignKey(x => x.SurveyOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__08B87AC5");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionOptionXref)
                    .HasForeignKey(x => x.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__07C4568C");
            });

            modelBuilder.Entity<SurveyQuestionXref>(entity =>
            {
                entity.ToTable("SurveyQuestionXref", "app");

                entity.Property(e => e.SurveyQuestionXrefId)
                    .HasColumnName("surveyQuestionXrefID")
                    .HasViewColumnName("surveyQuestionXrefID");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");

                entity.Property(e => e.SortOrder)
                    .IsRequired()
                    .HasColumnName("sortOrder")
                    .HasViewColumnName("sortOrder")
                    .HasMaxLength(5);

                entity.Property(e => e.SurveyId)
                    .HasColumnName("surveyID")
                    .HasViewColumnName("surveyID");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestionXref)
                    .HasForeignKey(x => x.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__03F3C5A8");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionXref)
                    .HasForeignKey(x => x.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__04E7E9E1");
            });

            modelBuilder.Entity<SurveyResponse>(entity =>
            {
                entity.ToTable("SurveyResponse", "app");

                entity.Property(e => e.SurveyResponseId)
                    .HasColumnName("surveyResponseID")
                    .HasViewColumnName("surveyResponseID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientID")
                    .HasViewColumnName("clientID");

                entity.Property(e => e.ResponseText)
                    .IsRequired()
                    .HasColumnName("responseText")
                    .HasViewColumnName("responseText")
                    .HasMaxLength(2000);

                entity.Property(e => e.SurveyId)
                    .HasColumnName("surveyID")
                    .HasViewColumnName("surveyID");

                entity.Property(e => e.SurveyOptionId)
                    .HasColumnName("surveyOptionID")
                    .HasViewColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__clien__7F2F108B");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__7E3AEC52");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.SurveyOptionId)
                    .HasConstraintName("FK__SurveyRes__surve__011758FD");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__002334C4");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag", "app");

                entity.HasIndex(x => x.TagName)
                    .HasName("UQ__Tag__288C38514A1A8275")
                    .IsUnique();

                entity.Property(e => e.TagId)
                    .HasColumnName("tagID")
                    .HasViewColumnName("tagID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasColumnName("tagName")
                    .HasViewColumnName("tagName")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
