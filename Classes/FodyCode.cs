using HgmViewer.Formats;
using MethodBoundaryAspect.Fody.Attributes;

namespace HgmViewer.Classes
{
    public sealed class KaitaiExceptionHookAttribute : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            if (args?.Instance is Hgm)
            {
                args.Exception.Data.Add("hgm", args.Instance);
            }
        }
    }
}