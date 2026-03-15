set SolutionDir=%~dp0
set Config=%1
set Top=%SolutionDir%

cd %Top%

if exist Jamrules.%Config% (
  echo Cleaning Jamrules.%Config%
  call del /f/q Jamrules
  call copy Jamrules.%Config% Jamrules

  cd %Top%
  call jam.bat clean

  cd %Top%
  del Jamrules.%Config%
  del Jamrules
)
