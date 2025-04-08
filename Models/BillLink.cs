using System.ComponentModel.DataAnnotations;
using fareShare.Models;

namespace fareShare.Models;


//Zian -> To link the bills with a list of user and make it easier to query with SQL later because we keep track of all the users that are linked to a bill

public class BillLink
{
    public int BillLinkId { get; set; }
    public int UserId { get; set; }
    public int BillId { get; set; }
    public Bill? Bill { get; set; }
}