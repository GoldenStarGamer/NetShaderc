#version 460
layout(location = 0) in vec2 texpos;
layout(location = 0) out vec4 FragColor;

layout(set = 1, binding = 0) uniform sampler2DArray texs;
layout(set = 1, binding = 1, std140) uniform Uniforms
{
	int texcount;
};

void main()
{
	vec4 color = vec4(0);
	for(int i = 0; i < texcount; i++)
	{
		color += texture(texs, vec3(texpos, i)) / float(texcount);
	}

	FragColor = color;
}