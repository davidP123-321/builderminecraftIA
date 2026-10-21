<Window x:Class="builderminecraftIA.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Minecraft Builder Pro" Height="900" Width="1360"
        WindowStartupLocation="CenterScreen">
    <Grid Margin="12">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="430"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <Border Grid.Column="0" Background="#171717" CornerRadius="12" Padding="14">
            <StackPanel>
                <TextBlock Text="Prompt" Foreground="White" FontSize="20" FontWeight="Bold"/>
                <TextBox x:Name="PromptTextBox"
                         Height="210"
                         Margin="0,10,0,0"
                         FontSize="14"
                         AcceptsReturn="True"
                         TextWrapping="Wrap"
                         VerticalScrollBarVisibility="Auto"
                         Text="Maison médiévale en pierre, 2 étages, 3 chambres, salon, cuisine, cave, entrée centrale, toit à pignon, style cozy, schéma propre, fonctionnel" />

                <TextBlock Text="Modèle Ollama" Foreground="White" FontSize="16" Margin="0,16,0,6"/>
                <ComboBox x:Name="ModelComboBox" SelectedIndex="0" FontSize="14">
                    <ComboBoxItem Content="llama3.1:8b"/>
                    <ComboBoxItem Content="qwen2.5-coder:7b"/>
                    <ComboBoxItem Content="deepseek-coder:6.7b"/>
                </ComboBox>

                <TextBlock Text="Taille" Foreground="White" FontSize="16" Margin="0,16,0,6"/>
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="X" Foreground="White" Width="18" VerticalAlignment="Center"/>
                    <TextBox x:Name="WidthBox" Width="90" Margin="6,0,20,0" Text="24"/>
                    <TextBlock Text="Z" Foreground="White" Width="18" VerticalAlignment="Center"/>
                    <TextBox x:Name="DepthBox" Width="90" Margin="6,0,0,0" Text="18"/>
                </StackPanel>

                <TextBlock Text="Style" Foreground="White" FontSize="16" Margin="0,16,0,6"/>
                <ComboBox x:Name="StyleComboBox" SelectedIndex="0" FontSize="14">
                    <ComboBoxItem Content="cozy"/>
                    <ComboBoxItem Content="medieval"/>
                    <ComboBoxItem Content="modern"/>
                    <ComboBoxItem Content="survival"/>
                    <ComboBoxItem Content="castle"/>
                </ComboBox>

                <Button x:Name="GenerateButton"
                        Content="Générer le schéma"
                        Height="46"
                        FontSize="16"
                        Margin="0,22,0,8"
                        Click="GenerateButton_Click"/>

                <Button x:Name="ExportButton"
                        Content="Exporter JSON"
                        Height="46"
                        FontSize="16"
                        Margin="0,0,0,8"
                        Click="ExportButton_Click"/>

                <Button x:Name="ExportSchemButton"
                        Content="Exporter .schem"
                        Height="46"
                        FontSize="16"
                        Margin="0,0,0,8"
                        Click="ExportSchemButton_Click"/>

                <ProgressBar x:Name="LoadingBar" Height="18" IsIndeterminate="True" Visibility="Collapsed" Margin="0,4,0,0"/>
                <TextBlock x:Name="StatusText" Foreground="#88d7ff" FontSize="13" Margin="0,12,0,0" Text="Prêt"/>
            </StackPanel>
        </Border>

        <Border Grid.Column="1" Background="#111111" CornerRadius="12" Padding="12">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="Auto"/>
                    <RowDefinition Height="*"/>
                    <RowDefinition Height="250"/>
                </Grid.RowDefinitions>

                <TextBlock Grid.Row="0" Foreground="White" FontSize="20" FontWeight="Bold" Text="Aperçu du schéma"/>

                <TabControl Grid.Row="1" Margin="0,10,0,10">
                    <TabItem Header="2D">
                        <ScrollViewer>
                            <Border Background="#0a0a0a" CornerRadius="8" Padding="8">
                                <Canvas x:Name="PreviewCanvas" Width="700" Height="700" Background="#0a0a0a"/>
                            </Border>
                        </ScrollViewer>
                    </TabItem>

                    <TabItem Header="3D">
                        <Border Background="#0a0a0a" CornerRadius="8" Padding="8">
                            <Viewport3D x:Name="ThreeDViewport" Width="700" Height="700"/>
                        </Border>
                    </TabItem>
                </TabControl>

                <TextBox x:Name="JsonOutput"
                         Grid.Row="2"
                         FontFamily="Consolas"
                         FontSize="12"
                         Foreground="#d9f9ff"
                         Background="#0d0d0d"
                         TextWrapping="Wrap"
                         VerticalScrollBarVisibility="Auto"
                         IsReadOnly="True"
                         Text="Le JSON du schéma apparaîtra ici..." />
            </Grid>
        </Border>
    </Grid>
</Window>
