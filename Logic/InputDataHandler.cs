using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryNET6Pages.Controllers
{
	public static class InputDataHandler
	{
		public static Tuple<bool, bool> CheckAdmin(string login, string password)
			=> MsSqlRepository<AdminPage>.IsEnterDataCorrect(login, password);

		public static bool IsDataParsedToInt(string data)
			=> data.Length == 0 ?
				true : int.TryParse(data, out var result) ?
			result >= 0 : true;

		public static bool IsDataParsedToLong(string data)
			=> data.Length == 0 ?
				true : int.TryParse(data, out var result) ?
			result >= 0 : true;
	}
}
