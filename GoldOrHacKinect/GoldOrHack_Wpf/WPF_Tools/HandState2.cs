 // ReSharper disable once CheckNamespace
namespace Microsoft.Samples.Kinect.DepthBasics.tools
{
    //
    // Summary:
    //     The state of a hand of a body.
    public enum HandState2
    {
        //
        // Summary:
        //     Undetermined hand state.
        Unknown = 0,
        //
        // Summary:
        //     Hand not tracked.
        NotTracked = 1,
        //
        // Summary:
        //     Open hand.
        Open = 2,
        //
        // Summary:
        //     Closed hand.
        Closed = 3,
        //
        // Summary:
        //     Lasso (pointer) hand.
        Lasso = 4
    }
}
