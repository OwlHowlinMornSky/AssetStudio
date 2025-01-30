#pragma once

using namespace System;

namespace FbxInterop {

public ref class Fbx {
public:

	static void AsUtilQuaternionToEuler(
		float qx, float qy, float qz, float qw,
		[Runtime::InteropServices::OutAttribute]float% vx,
		[Runtime::InteropServices::OutAttribute]float% vy,
		[Runtime::InteropServices::OutAttribute]float% vz
	);

	static void AsUtilEulerToQuaternion(
		float vx, float vy, float vz,
		[Runtime::InteropServices::OutAttribute]float% qx,
		[Runtime::InteropServices::OutAttribute]float% qy,
		[Runtime::InteropServices::OutAttribute]float% qz,
		[Runtime::InteropServices::OutAttribute]float% qw
	);

};

}
