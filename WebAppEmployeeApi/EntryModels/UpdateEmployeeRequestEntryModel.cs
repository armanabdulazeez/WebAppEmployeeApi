namespace WebAppEmployeeApi.EntryModels
{
    public class UpdateEmployeeRequestEntryModel
    {
        public EmployeeEntryModel Employee { get; set; }
        public List<EmployeeAddressEntryModel> Addresses { get; set; }
    }

}
