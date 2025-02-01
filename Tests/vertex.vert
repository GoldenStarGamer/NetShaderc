#version 460
layout(location = 0) in vec3 pos;
layout(location = 1) in vec2 texcoord;
layout(location = 0) out vec2 texpos;

layout(set = 0, binding = 0, std140) uniform Uniforms
{
	uniform mat4 model;
	uniform mat4 view;
	uniform mat4 project;
};

void main()
{
	gl_Position = vec4(pos, 1) * model * view * project;
	texpos = texcoord;
}