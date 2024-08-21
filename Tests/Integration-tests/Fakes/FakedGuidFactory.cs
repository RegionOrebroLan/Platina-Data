using System;
using System.Collections.Generic;
using System.Linq;
using RegionOrebroLan.Platina.Data;

namespace IntegrationTests.Fakes
{
	public class FakedGuidFactory : IGuidFactory
	{
		#region Properties

		public virtual ISet<Guid> Guids { get; } = new HashSet<Guid>
		{
			new("a89c8bc3-d8d2-40d8-b207-98886c621908"),
			new("107ff43b-2ced-49eb-a8fa-52050208c49b"),
			new("830d620a-1a2b-49f5-96fc-43b6d34764cf"),
			new("3bb3bdb3-fc62-4e01-9898-77f236c7b7a0"),
			new("8e5f1cbb-ac87-4521-95ed-32693775e48f"),
			new("1a16ca07-1dab-4b8c-87d2-1c765c309240"),
			new("d1d994a5-0c1a-4752-9030-a5298db9199c"),
			new("4ed79a74-cdc9-4b09-91c2-57e5912bb1cb"),
			new("80b4128c-728b-451d-be7e-48c0ea602135"),
			new("075543b0-3ce3-43e1-979b-9536e3252490"),
			new("b4a1f947-55c7-4128-8dc7-4db1441cca10"),
			new("84eaf100-5188-4821-b157-f6bd9d7a0e35"),
			new("26c2399f-9bfd-4259-8365-5f331230d53d"),
			new("b7aedb38-2efe-4b76-b54a-d338aabe42b0"),
			new("b2c8adb1-c274-46cd-8b1b-8df035b27186"),
			new("6f32bffa-9e29-4736-ab20-dcde07860fce"),
			new("2c74ff61-2478-4fa7-be33-1c6fa4436104"),
			new("1e397ae9-766c-4146-bc7c-a372f67b7a44"),
			new("b42ffbb7-2b38-4454-a811-78ec30d3fce6"),
			new("1fdac5da-f3fe-4fa3-b58b-536679791459")
		};

		protected internal virtual int Index { get; set; }

		#endregion

		#region Methods

		public virtual Guid Create()
		{
			var guid = this.Guids.ElementAt(this.Index);

			this.Index++;

			return guid;
		}

		public virtual void Reset()
		{
			this.Index = 0;
		}

		#endregion
	}
}