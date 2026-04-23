using Godot;
using System.Collections.Generic;

public partial class MainMenu : Control
{
	[Export] public PackedScene StartScene;

	private Button _newGameButton;
	private Button _continueButton;
	private Button _quitButton;
	private Button _gearButton;
	private Control _gearMenu;
	private Button _settingsButton;
	private Button _feedbackButton;
	private Label _titleLabel;
	private TextureRect _backgroundGradient;
	private PanelContainer _levelSelectMenu;
	private Button _resetSaveButton;
	private GameManager _gameManager;

	private readonly List<AnimatedMenuText> _animatedTexts = new();
	private readonly List<Button> _levelButtons = new();
	private readonly RandomNumberGenerator _random = new();
	private float _menuTime;

	private static readonly ColorPair[] TitlePalettes =
	{
		new(new Color(0.08f, 0.02f, 0.16f), new Color(0.14f, 0.95f, 0.28f)),
		new(new Color(0.02f, 0.04f, 0.18f), new Color(1.00f, 0.22f, 0.08f)),
		new(new Color(0.18f, 0.01f, 0.08f), new Color(0.08f, 0.90f, 1.00f)),
		new(new Color(0.10f, 0.02f, 0.20f), new Color(0.96f, 0.94f, 0.12f)),
		new(new Color(0.02f, 0.13f, 0.12f), new Color(1.00f, 0.10f, 0.72f)),
		new(new Color(0.22f, 0.07f, 0.02f), new Color(0.18f, 0.36f, 1.00f))
	};

	private static readonly ColorPair[] BackgroundPalettes =
	{
		new(new Color(0.00f, 0.00f, 0.00f), new Color(0.00f, 1.00f, 0.18f)),
		new(new Color(0.03f, 0.00f, 0.08f), new Color(0.84f, 1.00f, 0.00f)),
		new(new Color(0.00f, 0.03f, 0.12f), new Color(1.00f, 0.14f, 0.03f)),
		new(new Color(0.12f, 0.00f, 0.04f), new Color(0.00f, 0.92f, 0.86f)),
		new(new Color(0.00f, 0.08f, 0.04f), new Color(1.00f, 0.72f, 0.00f)),
		new(new Color(0.04f, 0.02f, 0.00f), new Color(0.68f, 0.08f, 1.00f))
	};

	public override void _Ready()
	{
		_gameManager = GetNodeOrNull<GameManager>("/root/GameManager");

		_newGameButton = GetNode<Button>("NewGame");
		_continueButton = GetNode<Button>("ContinueGame");
		_quitButton = GetNode<Button>("Exit");
		_titleLabel = GetNodeOrNull<Label>("Label");

		_gearButton = GetNodeOrNull<Button>("GearButton");
		_gearMenu = GetNodeOrNull<Control>("GearMenu");
		_settingsButton = GetNodeOrNull<Button>("GearMenu/VBoxContainer/SettingsButton");
		_feedbackButton = GetNodeOrNull<Button>("GearMenu/VBoxContainer/FeedbackButton");

		if (_titleLabel != null)
			_titleLabel.PivotOffset = _titleLabel.Size * 0.5f;

		if (_gearMenu != null)
			_gearMenu.Visible = false;

		_newGameButton.Pressed += OnNewGamePressed;
		_continueButton.Pressed += OnContinuePressed;
		_quitButton.Pressed += OnQuitPressed;

		if (_gearButton != null)
			_gearButton.Pressed += OnGearToggle;
		if (_settingsButton != null)
			_settingsButton.Pressed += OnSettings;
		if (_feedbackButton != null)
			_feedbackButton.Pressed += OnFeedback;

		_random.Randomize();
		ColorPair titlePalette = PickPalette(TitlePalettes);
		ColorPair backgroundPalette = PickDistinctBackgroundPalette(titlePalette);

		SetupBackgroundGradient(backgroundPalette);
		SetupLevelSelectMenu();
		SetupAnimatedText(_titleLabel, titlePalette.Dark, titlePalette.Bright, 0.0f, 8.0f, 0.0f, 2.4f, 1.25f, 1.6f, true);
		SetupAnimatedText(_newGameButton, new Color(0.12f, 0.02f, 0.22f), new Color(0.20f, 1.00f, 0.36f), 0.6f, 2.0f, 2.0f, 1.2f, 1.45f, 1.2f, false);
		SetupAnimatedText(_continueButton, new Color(0.06f, 0.04f, 0.20f), new Color(0.48f, 0.96f, 0.26f), 1.8f, 1.5f, 2.5f, -1.0f, 1.35f, 1.35f, false);
		SetupAnimatedText(_quitButton, new Color(0.16f, 0.02f, 0.18f), new Color(0.12f, 0.86f, 0.58f), 2.9f, 1.0f, 2.0f, 0.9f, 1.55f, 1.1f, false);
	}

	public override void _Process(double delta)
	{
		_menuTime += (float)delta;

		foreach (AnimatedMenuText animatedText in _animatedTexts)
			AnimateText(animatedText);
	}

	private ColorPair PickPalette(ColorPair[] palettes)
	{
		return palettes[_random.RandiRange(0, palettes.Length - 1)];
	}

	private ColorPair PickDistinctBackgroundPalette(ColorPair titlePalette)
	{
		ColorPair backgroundPalette = PickPalette(BackgroundPalettes);

		for (int i = 0; i < BackgroundPalettes.Length && ColorsAreClose(backgroundPalette.Bright, titlePalette.Bright); i++)
			backgroundPalette = BackgroundPalettes[(i + 1) % BackgroundPalettes.Length];

		return backgroundPalette;
	}

	private static bool ColorsAreClose(Color first, Color second)
	{
		float distance = Mathf.Abs(first.R - second.R) + Mathf.Abs(first.G - second.G) + Mathf.Abs(first.B - second.B);
		return distance < 0.45f;
	}

	private void SetupBackgroundGradient(ColorPair backgroundPalette)
	{
		if (_backgroundGradient != null)
			return;

		var gradient = new Gradient();
		gradient.SetColor(0, backgroundPalette.Dark);
		gradient.SetColor(1, backgroundPalette.Bright);

		var texture = new GradientTexture2D
		{
			Gradient = gradient,
			Width = 1024,
			Height = 1024,
			Fill = GradientTexture2D.FillEnum.Linear,
			FillFrom = Vector2.Zero,
			FillTo = Vector2.One,
			Repeat = GradientTexture2D.RepeatEnum.None
		};

		_backgroundGradient = new TextureRect
		{
			Name = "RuntimeBackgroundGradient",
			Texture = texture,
			MouseFilter = MouseFilterEnum.Ignore,
			ZIndex = -10,
			StretchMode = TextureRect.StretchModeEnum.Scale
		};

		_backgroundGradient.AnchorLeft = 0.0f;
		_backgroundGradient.AnchorTop = 0.0f;
		_backgroundGradient.AnchorRight = 1.0f;
		_backgroundGradient.AnchorBottom = 1.0f;
		_backgroundGradient.OffsetLeft = 0.0f;
		_backgroundGradient.OffsetTop = 0.0f;
		_backgroundGradient.OffsetRight = 0.0f;
		_backgroundGradient.OffsetBottom = 0.0f;

		AddChild(_backgroundGradient);

		CanvasItem decorImage = GetNodeOrNull<CanvasItem>("DecorImage");
		if (decorImage != null)
			MoveChild(_backgroundGradient, decorImage.GetIndex());
		else
			MoveChild(_backgroundGradient, 0);
	}

	private void SetupLevelSelectMenu()
	{
		_levelSelectMenu = new PanelContainer
		{
			Name = "RuntimeLevelSelectMenu",
			Visible = false,
			MouseFilter = MouseFilterEnum.Stop,
			ZIndex = 20
		};

		_levelSelectMenu.AnchorLeft = 0.5f;
		_levelSelectMenu.AnchorTop = 0.5f;
		_levelSelectMenu.AnchorRight = 0.5f;
		_levelSelectMenu.AnchorBottom = 0.5f;
		_levelSelectMenu.OffsetLeft = -150.0f;
		_levelSelectMenu.OffsetTop = -100.0f;
		_levelSelectMenu.OffsetRight = 150.0f;
		_levelSelectMenu.OffsetBottom = 170.0f;

		var vBox = new VBoxContainer
		{
			CustomMinimumSize = new Vector2(300.0f, 270.0f)
		};

		var title = new Label
		{
			Text = "LEVEL SELECT",
			HorizontalAlignment = HorizontalAlignment.Center
		};
		vBox.AddChild(title);

		int totalLevels = _gameManager?.TotalLevels ?? 3;
		for (int i = 1; i <= totalLevels; i++)
		{
			int levelNumber = i;
			var button = new Button();
			button.Pressed += () => OnLevelPressed(levelNumber);
			_levelButtons.Add(button);
			vBox.AddChild(button);
		}

		_resetSaveButton = new Button
		{
			Text = "DEBUG: RESET SAVE"
		};
		_resetSaveButton.Pressed += OnResetSavePressed;
		vBox.AddChild(_resetSaveButton);

		_levelSelectMenu.AddChild(vBox);
		AddChild(_levelSelectMenu);
		RefreshLevelSelectMenu();
	}

	private void RefreshLevelSelectMenu()
	{
		int highestUnlockedLevel = _gameManager?.HighestUnlockedLevel ?? 1;

		for (int i = 0; i < _levelButtons.Count; i++)
		{
			int levelNumber = i + 1;
			bool unlocked = levelNumber <= highestUnlockedLevel;
			Button button = _levelButtons[i];

			button.Disabled = !unlocked;
			button.Text = unlocked ? $"Level {levelNumber}" : $"Level {levelNumber} - locked";
		}

		if (_resetSaveButton != null)
			_resetSaveButton.Disabled = _gameManager == null;
	}

	private void SetupAnimatedText(Control source, Color darkColor, Color greenColor, float phase, float swayX, float swayY, float rotationDegrees, float pulseSpeed, float swaySpeed, bool splitIntoLetters)
	{
		if (source == null)
			return;

		string text = source switch
		{
			Label label => label.Text,
			Button button => button.Text,
			_ => string.Empty
		};

		if (string.IsNullOrEmpty(text))
			return;

		source.PivotOffset = source.Size * 0.5f;
		if (splitIntoLetters)
			HideSourceText(source);

		var animatedText = new AnimatedMenuText
		{
			Source = source,
			Text = text,
			DarkColor = darkColor,
			GreenColor = greenColor,
			BasePosition = source.Position,
			BaseRotation = source.RotationDegrees,
			Phase = phase,
			SwayX = swayX,
			SwayY = swayY,
			RotationDegrees = rotationDegrees,
			PulseSpeed = pulseSpeed,
			SwaySpeed = swaySpeed,
			SplitIntoLetters = splitIntoLetters
		};

		if (!splitIntoLetters)
		{
			_animatedTexts.Add(animatedText);
			return;
		}

		Font font = source.GetThemeFont("font");
		int fontSize = source.GetThemeFontSize("font_size");
		int outlineSize = source.GetThemeConstant("outline_size");
		if (outlineSize <= 0)
			outlineSize = source == _titleLabel ? 2 : 1;

		for (int i = 0; i < text.Length; i++)
		{
			if (char.IsWhiteSpace(text[i]))
				continue;

			var letter = new Label
			{
				Text = text[i].ToString(),
				MouseFilter = MouseFilterEnum.Ignore,
				ZIndex = 10
			};

			letter.AddThemeFontOverride("font", font);
			letter.AddThemeFontSizeOverride("font_size", fontSize);
			letter.AddThemeConstantOverride("outline_size", outlineSize);
			source.AddChild(letter);

			animatedText.Letters.Add(letter);
			animatedText.LetterSourceIndexes.Add(i);
		}

		_animatedTexts.Add(animatedText);
		LayoutLetters(animatedText);
	}

	private static void HideSourceText(Control source)
	{
		var transparent = new Color(0, 0, 0, 0);

		source.AddThemeColorOverride("font_color", transparent);
		source.AddThemeColorOverride("font_outline_color", transparent);

		if (source is Button)
		{
			source.AddThemeColorOverride("font_pressed_color", transparent);
			source.AddThemeColorOverride("font_hover_color", transparent);
			source.AddThemeColorOverride("font_hover_pressed_color", transparent);
			source.AddThemeColorOverride("font_disabled_color", transparent);
			source.AddThemeColorOverride("font_focus_color", transparent);
		}
	}

	private void AnimateText(AnimatedMenuText animatedText)
	{
		float sway = Mathf.Sin((_menuTime * animatedText.SwaySpeed) + animatedText.Phase);
		float counterSway = Mathf.Sin((_menuTime * animatedText.SwaySpeed * 0.83f) + (animatedText.Phase * 1.7f));

		animatedText.Source.Position = animatedText.BasePosition + new Vector2(sway * animatedText.SwayX, counterSway * animatedText.SwayY);
		animatedText.Source.RotationDegrees = animatedText.BaseRotation + sway * animatedText.RotationDegrees;

		if (animatedText.SplitIntoLetters)
		{
			LayoutLetters(animatedText);

			for (int i = 0; i < animatedText.Letters.Count; i++)
			{
				Label letter = animatedText.Letters[i];
				float pulse = (Mathf.Sin((_menuTime * animatedText.PulseSpeed) + animatedText.Phase + (animatedText.LetterSourceIndexes[i] * 0.38f)) + 1f) * 0.5f;
				Color color = animatedText.DarkColor.Lerp(animatedText.GreenColor, pulse);

				letter.AddThemeColorOverride("font_color", color);
				letter.AddThemeColorOverride("font_outline_color", new Color(color.R * 0.18f, color.G * 0.18f, color.B * 0.18f, 1f));
			}
		}
		else
		{
			float pulse = (Mathf.Sin((_menuTime * animatedText.PulseSpeed) + animatedText.Phase) + 1f) * 0.5f;
			Color color = animatedText.DarkColor.Lerp(animatedText.GreenColor, pulse);
			SetSourceTextColor(animatedText.Source, color);
		}
	}

	private static void SetSourceTextColor(Control source, Color color)
	{
		Color outlineColor = new Color(color.R * 0.18f, color.G * 0.18f, color.B * 0.18f, 1f);

		source.AddThemeColorOverride("font_color", color);
		source.AddThemeColorOverride("font_outline_color", outlineColor);

		if (source is Button)
		{
			source.AddThemeColorOverride("font_pressed_color", color);
			source.AddThemeColorOverride("font_hover_color", color.Lightened(0.2f));
			source.AddThemeColorOverride("font_hover_pressed_color", color.Lightened(0.15f));
			source.AddThemeColorOverride("font_disabled_color", color.Darkened(0.45f));
			source.AddThemeColorOverride("font_focus_color", color);
		}
	}

	private static void LayoutLetters(AnimatedMenuText animatedText)
	{
		Font font = animatedText.Source.GetThemeFont("font");
		int fontSize = animatedText.Source.GetThemeFontSize("font_size");
		Vector2 fullSize = MeasureText(font, animatedText.Text, fontSize);
		float x = (animatedText.Source.Size.X - fullSize.X) * 0.5f;
		float y = (animatedText.Source.Size.Y - fullSize.Y) * 0.5f;
		int letterIndex = 0;

		for (int i = 0; i < animatedText.Text.Length; i++)
		{
			string glyph = animatedText.Text[i].ToString();
			Vector2 glyphSize = MeasureText(font, glyph, fontSize);

			if (!char.IsWhiteSpace(animatedText.Text[i]))
			{
				Label letter = animatedText.Letters[letterIndex];
				letter.Position = new Vector2(x, y);
				letter.Size = glyphSize + new Vector2(4f, 4f);
				letterIndex++;
			}

			x += glyphSize.X;
		}
	}

	private static Vector2 MeasureText(Font font, string text, int fontSize)
	{
		return font.GetStringSize(text, HorizontalAlignment.Left, -1.0f, fontSize);
	}

	private void OnNewGamePressed()
	{
		_levelSelectMenu.Visible = false;

		if (_gameManager != null)
			_gameManager.StartNewGame();
		else
			GetTree().ChangeSceneToFile("res://Scenes/level_1.tscn");
	}

	private void OnContinuePressed()
	{
		RefreshLevelSelectMenu();
		_levelSelectMenu.Visible = !_levelSelectMenu.Visible;
	}

	private void OnLevelPressed(int levelNumber)
	{
		if (_gameManager != null)
			_gameManager.LoadLevel(levelNumber);
		else if (levelNumber == 1)
			GetTree().ChangeSceneToFile("res://Scenes/level_1.tscn");
	}

	private void OnResetSavePressed()
	{
		_gameManager?.ResetSave();
		RefreshLevelSelectMenu();
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}

	private void OnGearToggle()
	{
		if (_gearMenu != null)
			_gearMenu.Visible = !_gearMenu.Visible;
	}

	private void OnSettings()
	{
		GD.Print("Settings clicked (placeholder).");
	}

	private void OnFeedback()
	{
		GD.Print("Feedback clicked (placeholder).");
	}

	private sealed class AnimatedMenuText
	{
		public Control Source;
		public string Text = string.Empty;
		public readonly List<Label> Letters = new();
		public readonly List<int> LetterSourceIndexes = new();
		public Color DarkColor;
		public Color GreenColor;
		public Vector2 BasePosition;
		public float BaseRotation;
		public float Phase;
		public float SwayX;
		public float SwayY;
		public float RotationDegrees;
		public float PulseSpeed;
		public float SwaySpeed;
		public bool SplitIntoLetters;
	}

	private readonly struct ColorPair
	{
		public ColorPair(Color dark, Color bright)
		{
			Dark = dark;
			Bright = bright;
		}

		public readonly Color Dark;
		public readonly Color Bright;
	}
}
