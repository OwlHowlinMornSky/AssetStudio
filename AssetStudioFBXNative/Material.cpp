#include "Material.h"

namespace FbxInterop {

Material::Material(FbxSurfacePhong* _ptr) {
	_ref = _ptr;
}

Material::~Material() {
	_ref = nullptr;
}

void Material::LinkTexture(int dest, Texture^ texture, float offsetX, float offsetY, float scaleX, float scaleY) {
	return AsFbxLinkTexture(dest, texture->GetPtr(), _ref, offsetX, offsetY, scaleX, scaleY);
}

FbxSurfacePhong* Material::GetPtr() {
	return _ref;
}

}
