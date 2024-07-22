using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Yetibyte.CSharpToUmlet.Parsing;
using Yetibyte.CSharpToUmlet.UmletElements;

namespace Yetibyte.CSharpToUmlet;

public partial class MainViewModel(ISourceCodeParser sourceCodeParser) : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GenerateMarkupCommand))]
    private string _sourceCode = string.Empty;

    [ObservableProperty]
    private string _markup = string.Empty;  

    public bool CanGenerateMarkup() => !string.IsNullOrWhiteSpace(SourceCode);

    [RelayCommand(CanExecute = nameof(CanGenerateMarkup))]  
    public void GenerateMarkup()
    {
        IUmletElement umletElement;

        try
        {
            umletElement = sourceCodeParser.Parse(SourceCode);
        }
        catch (NoSupportedTypeFoundException)
        {
            MessageBox.Show("No supported type declaration could be found in the source code.", "No type found", MessageBoxButton.OK, MessageBoxImage.Information);

            return;
        }
        catch (Exception ex)
        {
            MessageBox.Show("An unexpected error has occurred.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Debug.WriteLine(ex);

            return;
        }

        Markup = umletElement.ToMarkup();
    }

    [RelayCommand]
    public void ClearMarkup() => Markup = string.Empty;
}
