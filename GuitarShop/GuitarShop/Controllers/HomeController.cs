using System.Threading.Tasks;
using System.Web.Mvc;
using GuitarShop.Models;
using GuitarShop.Providers;

namespace GuitarShop.Controllers
{
    public class HomeController : Controller
    {
        private const string Message = "Message";
        private const string SelectedGuitar = "SelectedGuitar";

        public GuitarDb GuitarDb { get; set; } = new GuitarDb();
        public ISender Sender { get; set; }

        public HomeController(ISender sender)
        {
            Sender = sender;
        }

        public ActionResult Index()
        {
            var message = Session[Message];
            if (!ReferenceEquals(message, null))
            {
                ViewBag.Message = message;
            }

            return View(GuitarDb.Guitars);
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Buy(int guitarId)
        {
            var selectedGuitar = GuitarDb.GetGuitarById(guitarId);

            if (ReferenceEquals(selectedGuitar, null))
            {
                return RedirectToAction("Error");
            }

            ViewBag.SelectedGuitar = selectedGuitar;
            Session[SelectedGuitar] = ViewBag.SelectedGuitar;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Buy(Purchase purchase)
        {
            ViewBag.SelectedWeapon = Session[SelectedGuitar];

            if (ModelState.IsValid)
            {
                var isInfoSentToClient = await Task<bool>.Factory.StartNew(() => Sender.SendInfoToClient(purchase));
                var message = $"Thank you, {purchase.Name}, for buying {purchase.GuitarName}. " +
                              $"On your e-mail - {purchase.Email} will be sent a mail with all required info.";

                if (!isInfoSentToClient)
                {
                    message = "Ooops. Mail wasn't sent on your e-mail. Try buy something again";
                }

                Session[Message] = message;
                return RedirectToAction("Index");
            }

            return View();   
        }
    }
}