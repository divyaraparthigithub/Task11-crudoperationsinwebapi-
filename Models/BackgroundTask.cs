using System.Data.SqlClient;
using System.Threading;

namespace Task11_crud_.Models
{
    public class BackgroundTask : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BackgroundTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                InsertDataIntoDatabase();
                await Task.Delay(TimeSpan.FromMinutes(1), cancel);
            }
        }

        private void InsertDataIntoDatabase()
        {
            var connectionString = "Data Source=DRAPARTH-L-5507\\SQLEXPRESS;Initial Catalog=Task11;User ID=sa;Password=Welcome2evoke@1234";
            var list = new List<(string Name, int ProductId,int GenderId,string Address,string Phone,string Email)>
        {
            ("alessa",1,2,"sydney","1234567890","alessa@gmail.com"),
            ("starc",2,1,"newsouthwhales","9876543210","alessa@gmail.com")


        };

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (var i in list)
                {
                    SqlCommand cmd = new SqlCommand("insert into Customer(Name, ProductId, GenderId, Address, Phone, Email)  VALUES (@Name, @ProductId,@GenderId,@Address,@Phone,@Email)", con);
                    cmd.Parameters.AddWithValue("@Name", i.Name);
                    cmd.Parameters.AddWithValue("@ProductId", i.ProductId);
                    cmd.Parameters.AddWithValue("@GenderId", i.GenderId);
                    cmd.Parameters.AddWithValue("@Address", i.Address);
                    cmd.Parameters.AddWithValue("@Phone", i.Phone);
                    cmd.Parameters.AddWithValue("@Email", i.Email);                 
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
