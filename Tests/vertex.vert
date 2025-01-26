#version 460
in vec3 pos;
in vec2 texcoord;
out vec2 texpos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 project;

void main()
{
	gl_Position = vec4(pos, 1) * model * view * project;
	texpos = texcoord;
}