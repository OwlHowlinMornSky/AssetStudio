#include "Texture.h"

namespace FbxInterop {

Texture::Texture(FbxFileTexture* _ptr) {
	_ref = _ptr;
}

Texture::~Texture() {
	_ref = nullptr;
}

bool Texture::IsValid() {
    return _ref != nullptr;
}

FbxFileTexture* Texture::GetPtr() {
	return _ref;
}

}

