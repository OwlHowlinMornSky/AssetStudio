#include "Node.h"

namespace FbxInterop {

Node::Node(FbxNode* _ptr) {
	_ref = _ptr;
}

Node::~Node() {
	_ref = nullptr;
}

int Node::AddMaterialToFrame(Material^ material) {
	return AsFbxAddMaterialToFrame(_ref, material->GetPtr());
}

void Node::SetFrameShadingModeToTextureShading() {
	AsFbxSetFrameShadingModeToTextureShading(_ref);
}

bool Node::IsValid() {
    return _ref != nullptr;
}

FbxNode* Node::GetPtr() {
	return _ref;
}

}
