#include "Fbx.h"

#include "api.h"

namespace FbxInterop {

void Fbx::AsUtilQuaternionToEuler(float qx, float qy, float qz, float qw, float% vx, float% vy, float% vz) {
	float rx = 0;
	float ry = 0;
	float rz = 0;
	::AsUtilQuaternionToEuler(qx, qy, qz, qw, &rx, &ry, &rz);
	vx = rx;
	vy = ry;
	vz = rz;
}

void Fbx::AsUtilEulerToQuaternion(float vx, float vy, float vz, float% qx, float% qy, float% qz, float% qw) {
	float rx = 0;
	float ry = 0;
	float rz = 0;
	float rw = 0;
	::AsUtilEulerToQuaternion(vx, vy, vz, &rx, &ry, &rz, &rw);
	qx = rx;
	qy = ry;
	qz = rz;
	qw = rw;
}

}
