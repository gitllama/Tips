//”ñ‘ª’èChipŽw’è
//if (Chip.ChipNo == "01") return;

Console.WriteLine(Chip.FilePath);
//Chip.CheckFile();

Chip["Dark", "Ave"]
    .Intermediate(x => x.FilterMedianBayer()["Normal"].StaggerR())
    .Filter(x => x["Normal"].StaggerR())
    ["Active"]
    .Signal()
    .Defect(64,127,255);
