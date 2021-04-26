# This does not seem to be possible because Unity uses Mono.
# Try again when they support the net5/6 runtime

function HelloUnity() {
    param(
        [Parameter(Mandatory)]
        [ParameterType]
        $gameObject
    )
    Write-Output $gameObject.transform
}