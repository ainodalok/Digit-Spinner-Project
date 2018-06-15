Shader "Custom/Stencil_Shader" {

Properties {
	_Stencil	("Stencil ID", int) = 0
}

SubShader {
	Tags 
	{
		"Queue"="Geometry-1"
	}
	ColorMask 0
	ZWrite Off
	Stencil
	{
		Ref [_Stencil]
		Comp Always
		Pass Replace
	}
	
	Pass {

	}
}

}