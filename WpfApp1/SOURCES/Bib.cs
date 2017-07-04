//https://stackoverflow.com/questions/4547320/using-folderbrowserdialog-in-wpf-application/4547385#comment76194007_4547385
//var dialog = new System.Windows.Forms.FolderBrowserDialog();
//System.Windows.Forms.DialogResult result = dialog.ShowDialog();

////https://stackoverflow.com/questions/422090/in-c-sharp-check-that-filename-is-possibly-valid-not-that-it-exists/11636052#11636052
//public bool IsPathValidRootedLocal(String pathString)
//{
//    Uri pathUri;
//    Boolean isValidUri = Uri.TryCreate(pathString, UriKind.Absolute, out pathUri);
//    return isValidUri && pathUri != null && pathUri.IsLoopback;
////}

//https://stackoverflow.com/questions/13267657/adding-a-listboxitem-in-a-listbox-in-c
//ListBoxItem itm = new ListBoxItem();
//itm.Content = "some text";

//listbox.Items.Add(itm);

//https://stackoverflow.com/questions/18315786/confirmation-box-in-c-sharp-wpf
//MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
//        if (messageBoxResult == MessageBoxResult.Yes)
// //...........

////https://stackoverflow.com/questions/43113833/listbox-duplicate-check

//foreach (var item in ListBox1.Items)
//            {
//               if (item.Text.Contains(Combobox1.SelectedText.ToString()))
//               {
//                   //select item in the ListBox
//                   debugMsg("Duplicate","");
//                   break;
//               } else {
//                ListBox1.Items.Add(Combobox1.SelectedItem); }
//            }

////if ( listBox1.Items.Cast<string>().Contains(comboBox1.SelectedItem.ToString() ) )
////{
////    MessageBox.Show( "duplicate" );
//}
//else
//{
//    listBox1.Items.Add(comboBox1.SelectedItem );
//}

//https://social.msdn.microsoft.com/Forums/vstudio/en-US/f3fd4a79-938d-4caf-955b-082a8acf5946/how-to-display-folder-contents-in-a-listbox?forum=vbgeneral
//Button 1 puts the files in the listbox

//Code Snippet

//Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

//ListBox1.Tag = "C:\test" 'Keep track of the directory

//'Get the files and add them to the listbox

//For Each File As String In My.Computer.FileSystem.GetFiles("C:\test")

//'Only add the file names and not the directory

//'Use ListBox1.Items.Add(File) if you want the directory too

//ListBox1.Items.Add(My.Computer.FileSystem.GetFileInfo(File).Name)

//Next

//End Sub



//Button 2 puts the selected file in the listbox into a label and writes it to a text file
//Code Snippet

//Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

//'Add path & filename to label

//Label1.Text = My.Computer.FileSystem.CombinePath(ListBox1.Tag, ListBox1.Text)

//'Create a streamwriter to write to the file

//Dim SW As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter("c:\test\test.txt", True)

//'Write path & file name to the text file

//SW.WriteLine(Label1.Text) 'or My.Computer.FileSystem.CombinePath(ListBox1.Tag, ListBox1.Text)

//SW.Close() 'Close streamwriter

//End Sub