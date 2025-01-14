﻿using Content.Shared.CrewManifest;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.CartridgeLoader.Cartridges;

[GenerateTypedNameReferences]
public sealed partial class CrewManifestUiFragment : BoxContainer
{
    private CrewManifestEntries? _entryCache;

    public CrewManifestUiFragment()
    {
        RobustXamlLoader.Load(this);

        StationName.AddStyleClass("LabelBig");
        Orientation = LayoutOrientation.Vertical;
        HorizontalExpand = true;
        VerticalExpand = true;

        TextFilter.OnTextEntered += e => UpdateState(StationName.Text ?? "", _entryCache);
        TextFilter.OnTextChanged += e => UpdateState(StationName.Text ?? "", _entryCache);
    }

    public void UpdateState(string stationName, CrewManifestEntries? entries)
    {
        CrewManifestListing.DisposeAllChildren();
        CrewManifestListing.RemoveAllChildren();

        StationNameContainer.Visible = entries != null;
        StationName.Text = stationName;

        _entryCache = entries;

        if (entries == null)
            return;

        var entryList = FilterEntries(entries);
        CrewManifestListing.AddCrewManifestEntries(entryList);
    }

    private CrewManifestEntries FilterEntries(CrewManifestEntries entries)
    {
        if (string.IsNullOrWhiteSpace(TextFilter.Text))
        {
            return entries;
        }

        var result = new CrewManifestEntries();
        foreach (var entry in entries.Entries)
        {
            if (entry.Name.Contains(TextFilter.Text, StringComparison.OrdinalIgnoreCase)
                || entry.JobPrototype.Contains(TextFilter.Text, StringComparison.OrdinalIgnoreCase)
                || entry.JobTitle.Contains(TextFilter.Text, StringComparison.OrdinalIgnoreCase))
            {
                result.Entries.Add(entry);
            }
        }

        return result;
    }
}
