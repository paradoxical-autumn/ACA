:: Run resonite without monkeyloader.
:: created for windows users with resonite at the default path. (ie, me.)

set prdx_CWD=%cd%
cd /D "C:\Program Files (x86)\Steam\steamapps\common\Resonite"

Resonite.exe -Screen -SkipIntroTutorial -Invisible -LoadAssembly "C:\Users\paradox\source\repos\ArbitraryComponentAccess\ArbitraryComponentAccess\bin\Debug\net9.0\ArbitraryComponentAccess.dll" --hookfxr-disable

cd /D %prdx_CWD%

:: --doorstop-enabled false -DataPath "C:/res" -CachePath "C:/res"
