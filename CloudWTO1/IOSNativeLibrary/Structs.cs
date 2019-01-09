using System;
using ObjCRuntime;
using Foundation;
namespace IOSNativeLibrary
{
	[Native]
public enum JPAuthorizationOptions : uint
{
    None = 0,
    Badge = (1 << 0),
    Sound = (1 << 1),
    Alert = (1 << 2)
}

}
