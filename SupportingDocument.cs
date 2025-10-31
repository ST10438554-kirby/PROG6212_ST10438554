// App.xam
<Application x:Class="LecturerClaimSystem.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        
    </Application.Resources>
</Application>

// claim.cs
using System;
using System.Collections.ObjectModel;

namespace LecturerClaimSystem
{
    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Claim
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string Notes { get; set; }
        public ObservableCollection<string> UploadedFiles { get; set; } = new ObservableCollection<string>();
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public decimal TotalAmount => (decimal)HoursWorked * HourlyRate;
    }
}

// MainWindow.xaml
<Window x:Class="LecturerClaimSystem.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Lecturer Claim System" Height="600" Width="900">
    <Grid Margin="10">
        <TabControl>
            <!-- Lecturer Tab -->
            <TabItem Header="Lecturer - Submit Claim">
                <ScrollViewer>
                <StackPanel Margin="12" VerticalAlignment="Top" HorizontalAlignment="Left" Width="600">
                    <TextBlock Text="Submit a Claim" FontSize="20" FontWeight="Bold" Margin="0,0,0,10"/>
                    
                    <StackPanel Orientation="Horizontal" Margin="0,4">
                        <TextBlock Text="Lecturer Name:" Width="120" VerticalAlignment="Center"/>
                        <TextBox x:Name="TxtLecturerName" Width="300"/>
                    </StackPanel>

                    <StackPanel Orientation="Horizontal" Margin="0,4">
                        <TextBlock Text="Hours Worked:" Width="120" VerticalAlignment="Center"/>
                        <TextBox x:Name="TxtHoursWorked" Width="100" />
                    </StackPanel>

                    <StackPanel Orientation="Horizontal" Margin="0,4">
                        <TextBlock Text="Hourly Rate (R):" Width="120" VerticalAlignment="Center"/>
                        <TextBox x:Name="TxtHourlyRate" Width="100" />
                    </StackPanel>

                    <TextBlock Text="Additional Notes:" Margin="0,8"/>
                    <TextBox x:Name="TxtNotes" Height="80" Width="500" TextWrapping="Wrap" AcceptsReturn="True"/>

                    <StackPanel Orientation="Horizontal" Margin="0,10" VerticalAlignment="Center">
                        <Button x:Name="BtnUpload" Click="BtnUpload_Click" Width="120" Height="30">Upload Document</Button>
                        <TextBlock x:Name="TxtUploadedFiles" Margin="10,0,0,0" VerticalAlignment="Center"/>
                    </StackPanel>

                    <StackPanel Orientation="Horizontal" Margin="0,12">
                        <Button x:Name="BtnSubmit" Click="BtnSubmit_Click" Width="120" Height="36">Submit Claim</Button>
                        <TextBlock x:Name="LblSubmitStatus" Margin="12,6,0,0" VerticalAlignment="Center"/>
                    </StackPanel>

                    <Separator Margin="0,18"/>
                    <TextBlock Text="Your Submitted Claims" FontSize="16" FontWeight="Bold" Margin="0,6"/>
                    <DataGrid x:Name="DgLecturerClaims" Width="820" Height="220" 
                              AutoGenerateColumns="False" IsReadOnly="True" ItemsSource="{Binding}">
                        <DataGrid.Columns>
                            <DataGridTextColumn Header="ID" Binding="{Binding Id}" Width="180"/>
                            <DataGridTextColumn Header="Lecturer" Binding="{Binding LecturerName}" Width="120"/>
                            <DataGridTextColumn Header="Hours" Binding="{Binding HoursWorked}" Width="60"/>
                            <DataGridTextColumn Header="Rate (R)" Binding="{Binding HourlyRate}" Width="80"/>
                            <DataGridTextColumn Header="Total (R)" Binding="{Binding TotalAmount}" Width="80"/>
                            <DataGridTextColumn Header="Status" Binding="{Binding Status}" Width="80"/>
                            <DataGridTextColumn Header="Files" Binding="{Binding UploadedFiles.Count}" Width="60"/>
                            <DataGridTextColumn Header="Notes" Binding="{Binding Notes}" Width="180"/>
                        </DataGrid.Columns>
                    </DataGrid>
                </StackPanel>
                </ScrollViewer>
            </TabItem>

            <!-- Manager Tab -->
            <TabItem Header="Coordinator / Manager">
                <StackPanel Margin="12">
                    <TextBlock Text="Pending Claims" FontSize="20" FontWeight="Bold" Margin="0,0,0,8"/>

                    <DataGrid x:Name="DgPendingClaims" Width="850" Height="380" AutoGenerateColumns="False" ItemsSource="{Binding}">
                        <DataGrid.Columns>
                            <DataGridTextColumn Header="ID" Binding="{Binding Id}" Width="200"/>
                            <DataGridTextColumn Header="Lecturer" Binding="{Binding LecturerName}" Width="120"/>
                            <DataGridTextColumn Header="Hours" Binding="{Binding HoursWorked}" Width="60"/>
                            <DataGridTextColumn Header="Rate (R)" Binding="{Binding HourlyRate}" Width="80"/>
                            <DataGridTextColumn Header="Total (R)" Binding="{Binding TotalAmount}" Width="80"/>
                            <DataGridTextColumn Header="Notes" Binding="{Binding Notes}" Width="180"/>
                            <DataGridTextColumn Header="Files" Binding="{Binding UploadedFiles.Count}" Width="60"/>
                            <DataGridTextColumn Header="Status" Binding="{Binding Status}" Width="80"/>
                            <DataGridTemplateColumn Header="Actions" Width="120">
                                <DataGridTemplateColumn.CellTemplate>
                                    <DataTemplate>
                                        <StackPanel Orientation="Horizontal">
                                            <Button Content="Approve" Margin="2" Padding="6,2" Click="BtnApprove_Click" Tag="{Binding Id}"/>
                                            <Button Content="Reject" Margin="2" Padding="6,2" Click="BtnReject_Click" Tag="{Binding Id}"/>
                                        </StackPanel>
                                    </DataTemplate>
                                </DataGridTemplateColumn.CellTemplate>
                            </DataGridTemplateColumn>
                        </DataGrid.Columns>
                    </DataGrid>

                    <StackPanel Orientation="Horizontal" Margin="0,8">
                        <TextBlock Text="Filter:" VerticalAlignment="Center" Margin="0,4"/>
                        <ComboBox x:Name="CbFilter" Width="140" Margin="8,0" SelectionChanged="CbFilter_SelectionChanged">
                            <ComboBoxItem Content="All" IsSelected="True"/>
                            <ComboBoxItem Content="Pending"/>
                            <ComboBoxItem Content="Approved"/>
                            <ComboBoxItem Content="Rejected"/>
                        </ComboBox>
                    </StackPanel>
                </StackPanel>
            </TabItem>
        </TabControl>
    </Grid>
</Window>

// MainWindow.xaml.cs
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace LecturerClaimSystem
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Claim> Claims = new ObservableCollection<Claim>();
        private ObservableCollection<Claim> FilteredClaims = new ObservableCollection<Claim>();
        private const long MAX_FILE_BYTES = 5 * 1024 * 1024; // 5 MB
        private readonly string[] AllowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".doc" };
        private string lastUploadedFilePath = null;

        public MainWindow()
        {
            InitializeComponent();
            // bind DataGrids
            DgLecturerClaims.ItemsSource = Claims;
            DgPendingClaims.ItemsSource = Claims;
            // create upload folder
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles"));
            // add demo claim (optional)
            Claims.Add(new Claim { LecturerName = "Demo Lecturer", HoursWorked = 2, HourlyRate = 250m, Notes = "Demo claim" });
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                dlg.Filter = "Documents|*.pdf;*.docx;*.doc;*.xlsx";
                bool? res = dlg.ShowDialog();
                if (res == true)
                {
                    var fi = new FileInfo(dlg.FileName);

                    // size limit
                    if (fi.Length > MAX_FILE_BYTES)
                    {
                        MessageBox.Show($"File is too large. Limit is {MAX_FILE_BYTES / (1024 * 1024)} MB.", "Upload error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // extension check
                    if (!AllowedExtensions.Contains(fi.Extension.ToLower()))
                    {
                        MessageBox.Show("File type not allowed.", "Upload error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // copy to local folder with unique name
                    string destFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadedFiles");
                    string destFileName = $"{Guid.NewGuid()}{fi.Extension}";
                    string destFilePath = Path.Combine(destFolder, destFileName);
                    File.Copy(dlg.FileName, destFilePath);

                    lastUploadedFilePath = destFilePath;
                    TxtUploadedFiles.Text = $"Uploaded: {fi.Name}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // validation
                if (string.IsNullOrWhiteSpace(TxtLecturerName.Text))
                {
                    MessageBox.Show("Please enter your name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(TxtHoursWorked.Text, out double hours) || hours < 0)
                {
                    MessageBox.Show("Please provide a valid number for hours worked.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(TxtHourlyRate.Text, out decimal rate) || rate < 0)
                {
                    MessageBox.Show("Please provide a valid hourly rate.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var claim = new Claim
                {
                    LecturerName = TxtLecturerName.Text.Trim(),
                    HoursWorked = hours,
                    HourlyRate = rate,
                    Notes = TxtNotes.Text?.Trim()
                };

                if (!string.IsNullOrEmpty(lastUploadedFilePath))
                {
                    // store filename (not full path) in the claim, but keep the file in folder
                    claim.UploadedFiles.Add(Path.GetFileName(lastUploadedFilePath));
                    // reset uploaded file pointer after associating it
                    lastUploadedFilePath = null;
                    TxtUploadedFiles.Text = "";
                }

                Claims.Add(claim);

                // clear form
                TxtLecturerName.Text = "";
                TxtHoursWorked.Text = "";
                TxtHourlyRate.Text = "";
                TxtNotes.Text = "";
                LblSubmitStatus.Text = "Claim submitted successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to submit claim: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Claim GetClaimByIdTag(object tag)
        {
            if (tag is null) return null;
            if (Guid.TryParse(tag.ToString(), out Guid id))
            {
                return Claims.FirstOrDefault(c => c.Id == id);
            }
            return null;
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                var claim = GetClaimByIdTag(button?.Tag);
                if (claim == null) return;

                if (claim.Status != ClaimStatus.Pending)
                {
                    MessageBox.Show("Claim is already processed.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var answer = MessageBox.Show("Approve this claim?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    claim.Status = ClaimStatus.Approved;
                    RefreshGrids();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving claim: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                var claim = GetClaimByIdTag(button?.Tag);
                if (claim == null) return;

                if (claim.Status != ClaimStatus.Pending)
                {
                    MessageBox.Show("Claim is already processed.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var answer = MessageBox.Show("Reject this claim?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    claim.Status = ClaimStatus.Rejected;
                    RefreshGrids();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rejecting claim: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshGrids()
        {
            // force refresh by resetting ItemsSource (simple approach)
            DgLecturerClaims.ItemsSource = null;
            DgLecturerClaims.ItemsSource = Claims;
            DgPendingClaims.ItemsSource = null;
            DgPendingClaims.ItemsSource = Claims;
        }

        private void CbFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selected = (CbFilter.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString();
            if (selected == "All")
            {
                DgPendingClaims.ItemsSource = Claims;
            }
            else if (Enum.TryParse<ClaimStatus>(selected ?? "Pending", out ClaimStatus st))
            {
                DgPendingClaims.ItemsSource = new ObservableCollection<Claim>(Claims.Where(c => c.Status == st));
            }
        }
    }
}
