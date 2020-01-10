$paths = Get-ChildItem -include *.csproj -Recurse
foreach($pathobject in $paths) 
{
    $path = $pathobject.fullname
    $doc = New-Object System.Xml.XmlDocument
    $doc.Load($path)
    $child = $doc.CreateElement("ProjectGuid")
    $child.InnerText = [guid]::NewGuid().ToString().ToUpper()
    $node = $doc.SelectSingleNode("//Project/PropertyGroup")
    $node.AppendChild($child)
    $doc.Save($path)
}