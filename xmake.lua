set_project("GDINFMG")
add_rules("plugin.vsxmake.autoupdate")
set_arch(os.arch())
add_rules("mode.debug","mode.release")
set_defaultmode("debug")

outputdir = ""
if is_mode("debug") then
	outputdir = "Debug-$(arch)"
elseif is_mode("release") then
	outputdir = "Release-$(arch)"
end


target("GDINFMG")
	set_kind("binary")
	set_languages("c++20")
	set_runtimes("MT")

	set_targetdir("bin/".. outputdir .. "/GDINFMG")
	set_objectdir("bin-int/".. outputdir .. "/GDINFMG")

	add_headerfiles("Source/**.h")
	add_files("**.cpp")
	
	add_defines("_CRT_SECURE_NO_WARNINGS")


	if is_os("windows") then
		set_languages("c++20")
		add_defines("WINVER=0x0A00")
		add_defines("_WIN32_WINNT=0x0A00") 
		add_defines("PLATFORM_WINDOWS")
		--add_defines("GLFW_INCLUDE_NONE")
	end

	-- after_build(function (target) 
	-- 	local outputdir = ""
	-- 	if is_mode("debug") then
	-- 		outputdir = "Debug-$(arch)"
	-- 	elseif is_mode("release") then
	-- 		outputdir = "Release-$(arch)"
	-- 	end
	-- 	print("hello")
    --     os.cp("bin/" .. outputdir.."/Aeat/Aeat.dll", path.join("bin/" .. outputdir.. "/Sandbox", path.basename("bin/" .. outputdir.."/Aeat/Aeat.dll") .. ".dll"))  -- Copy the built target to the destination
	-- end)

	if is_mode("debug") then
		set_runtimes("MTd")
		add_defines("DEBUG")
		set_symbols("debug")
	elseif is_mode("release") then 
		set_runtimes("MT")
		add_defines("RELEASE")
		set_optimize("fast")
	end

