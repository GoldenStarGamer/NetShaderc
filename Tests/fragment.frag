#version 460
in vec2 texpos;
out vec4 FragColor;

uniform sampler2DArray texs;
uniform int texcount;

void main()
{
	vec4 color = vec4(0);
	for(int i = 0; i < texcount; i++)
	{
		color += texture(texs, vec3(texpos, i)) / float(texcount);
	}

	FragColor = color;
}