using System.ComponentModel.DataAnnotations.Schema;
using W.O.API.Domain.Common;
using W.O.API.Domain.Common.Exceptions;

namespace W.O.API.Domain
{
    public class Visit : Base
    {
        public Visit(Guid workOrderId, string assigneeFullName, DateTime assignedFrom, List<Part> parts)
        {
            ArgumentException.ThrowIfNullOrEmpty(assigneeFullName, nameof(assigneeFullName));
            CustomArgumentException.ThrowIfDefault(assignedFrom, nameof(assignedFrom));
            CustomArgumentException.ThrowIfDefault(workOrderId, nameof(workOrderId));
            InvalidVisitException.ThrowIfInvalidNumberOfParts(parts, 3);

            this.WorkOrderId = workOrderId;
            this.AssigneeFullName = assigneeFullName;
            this.AssignedFrom = assignedFrom;
            this.Parts = parts;
        }

        private Visit() { }

        [NotMapped]
        public int TotalParts => this.Parts.Count;

        public ICollection<Part> Parts { get; private set; } = new List<Part>();
        public Guid WorkOrderId { get; private set; }
        public string AssigneeFullName { get; private set; }
        public DateTime AssignedFrom { get; private set; }

        public Visit Update(string? assigneeFullName, DateTime? assignedFrom)
        {
            AssignedFrom = assignedFrom ?? this.AssignedFrom;
            AssigneeFullName = assigneeFullName ?? this.AssigneeFullName;
            return this;
        }
    }
}

