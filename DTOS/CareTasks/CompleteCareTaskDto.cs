namespace FloraBackend.DTOs.CareTasks
{
    public class CompleteCareTaskDto
    {
        // Optional: when the task was done. If empty, the backend uses "now".
        public DateTime? DoneAt { get; set; }
    }
}