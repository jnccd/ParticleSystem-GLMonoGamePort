sampler s0;
float2 WindowSize;
float2 Pos;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 Original = tex2D(s0, coords);

	float dx = coords.x * WindowSize.x - Pos.x;
	float dy = coords.y * WindowSize.y - Pos.y;
	float distance = sqrt(dx * dx + dy * dy);

	Original.rgba = 1 - sqrt(distance) / 15;

    return Original;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
