#pragma once
namespace iac_dll
{
	class CaptureEngine
	{
	public:
		using error_callback_t = void(*)(const std::string&);
		using capture_events_callback_t = void(*)(std::unique_ptr<Event>);
		
		CaptureEngine(capture_events_callback_t, error_callback_t);
		~CaptureEngine();
		
		CaptureEngine(CaptureEngine&& other) noexcept = default;
		CaptureEngine(CaptureEngine& other) = delete;
		CaptureEngine& operator=(CaptureEngine&& other) noexcept = default;
		CaptureEngine& operator=(const CaptureEngine& other) = delete;
		
		void start_capture() const;
		void stop_capture() const;

	private:
		const static UINT WM_STARTCAPTURE = WM_USER;
		const static UINT WM_STOPCAPTURE = WM_USER + 1;

		capture_events_callback_t capture_events_callback_ = nullptr;
		error_callback_t error_callback_ = nullptr;

		std::chrono::time_point<std::chrono::high_resolution_clock> time_of_start_of_recording_;

		std::unique_ptr<DWORD> window_thread_id_ = nullptr;

		static bool register_raw_input_stuff(HWND hwnd);
		static bool unregister_raw_input_stuff();
		static bool save_engine_ptr_to_window(const HWND&, const LPARAM&);
		static bool get_engine_ptr_from_window(const HWND&, iac_dll::CaptureEngine**);
		static LRESULT CALLBACK capture_window_wnd_proc(HWND, UINT, WPARAM, LPARAM);
		static bool register_window_class_if_needed(const wchar_t* class_name);
		static DWORD WINAPI capture_window_main_loop_thread(LPVOID);

		void handle_keyboard_event_capture(RAWKEYBOARD data) const;
		void handle_mouse_event_capture(RAWMOUSE data) const;
		void fake_mouse_event_for_initial_pos() const;
	};

}