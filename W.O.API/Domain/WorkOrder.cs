using System.ComponentModel.DataAnnotations.Schema;
using W.O.API.Domain.Common;
using W.O.API.Domain.Common.Exceptions;


namespace W.O.API.Domain
{
    public class WorkOrder : Base
    {
        public WorkOrder(string title, string description, string phone, string email, DateTime startAt, DateTime finishAt)
        {
            ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));
            ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));
            ArgumentException.ThrowIfNullOrEmpty(phone, nameof(phone));
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
            CustomArgumentException.ThrowIfDefault(startAt, nameof(startAt));
            CustomArgumentException.ThrowIfDefault(finishAt, nameof(finishAt));
            

            Title = title;
            Description = description;
            Phone = phone;
            Email = email;
            StartAt = startAt;
            FinishAt = finishAt;
        }

        private WorkOrder() { }

        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public DateTime StartAt { get; private set; }
        public DateTime FinishAt { get; private set; }

        [NotMapped]
        public int TotalVisits => this.Visits.Count;

        [NotMapped]
        public int TotalParts
        {
            get
            {
                int x = 0; foreach (var visit in this.Visits)
                {
                    x += visit.TotalParts;
                }
                return x;
            }
        }

        public ICollection<Visit> Visits { get; private set; } = new List<Visit> { };

        public WorkOrder Update(string? title, string? description, string? phone, string? email, DateTime? startAt, DateTime? finishAt)
        {
            Title = title ?? this.Title;
            Description = description ?? this.Description;
            Phone = phone ?? this.Phone;
            Email = email ?? this.Email;
            StartAt = startAt ?? this.StartAt;
            FinishAt = finishAt ?? this.FinishAt;

            return this;
         }
        }
    }

