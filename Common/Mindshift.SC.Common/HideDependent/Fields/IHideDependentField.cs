using System.Web.UI;
namespace Mindshift.SC.Common.HideDependent.Fields {
	public interface IHideDependentField {
		string Source { get; set; }
		string Class { get; set; }
		AttributeCollection Attributes { get; }
	}
}