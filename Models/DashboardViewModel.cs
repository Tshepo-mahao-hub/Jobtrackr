namespace JobTrackr.Models;
public class DashboardViewModel
{
    public List<JobApplication> Applications { get; set; } = [];
    public int Total => Applications.Count; public int Interviews => Applications.Count(x => x.Status == ApplicationStatus.Interviewing || x.Interviews.Any());
    public int Offers => Applications.Count(x => x.Status == ApplicationStatus.Offer); public int Rejections => Applications.Count(x => x.Status == ApplicationStatus.Rejected);
    public int Pending => Applications.Count(x => x.Status is ApplicationStatus.Applied or ApplicationStatus.Interviewing); public int SuccessRate => Total == 0 ? 0 : (int)Math.Round(Offers * 100.0 / Total);
}
