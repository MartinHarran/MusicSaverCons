namespace MusicSaver.Forms
{
    partial class FullPlayList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvPlaylists = new System.Windows.Forms.DataGridView();
            this.PLName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PLID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PLTracks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PLUri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bRefreshPlaylists = new System.Windows.Forms.Button();
            this.dgvTracksOnList = new System.Windows.Forms.DataGridView();
            this.trTrackTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trTrackID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bPlayAndRecord = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddGenres = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bUKUpdate = new System.Windows.Forms.Button();
            this.txtUKEnd = new System.Windows.Forms.TextBox();
            this.txtUKStart = new System.Windows.Forms.TextBox();
            this.bTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracksOnList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "My Playlists";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(475, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Songs On List";
            // 
            // dgvPlaylists
            // 
            this.dgvPlaylists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlaylists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PLName,
            this.PLID,
            this.PLTracks,
            this.PLUri});
            this.dgvPlaylists.Location = new System.Drawing.Point(15, 33);
            this.dgvPlaylists.Name = "dgvPlaylists";
            this.dgvPlaylists.Size = new System.Drawing.Size(414, 395);
            this.dgvPlaylists.TabIndex = 2;
            this.dgvPlaylists.SelectionChanged += new System.EventHandler(this.dgvPlaylists_SelectionChanged);
            // 
            // PLName
            // 
            this.PLName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.PLName.HeaderText = "Name";
            this.PLName.Name = "PLName";
            this.PLName.Width = 60;
            // 
            // PLID
            // 
            this.PLID.HeaderText = "ID";
            this.PLID.Name = "PLID";
            // 
            // PLTracks
            // 
            this.PLTracks.HeaderText = "No Tracks";
            this.PLTracks.Name = "PLTracks";
            // 
            // PLUri
            // 
            this.PLUri.HeaderText = "Uri";
            this.PLUri.Name = "PLUri";
            // 
            // bRefreshPlaylists
            // 
            this.bRefreshPlaylists.Location = new System.Drawing.Point(79, 4);
            this.bRefreshPlaylists.Name = "bRefreshPlaylists";
            this.bRefreshPlaylists.Size = new System.Drawing.Size(75, 23);
            this.bRefreshPlaylists.TabIndex = 3;
            this.bRefreshPlaylists.Text = "Refresh";
            this.bRefreshPlaylists.UseVisualStyleBackColor = true;
            this.bRefreshPlaylists.Click += new System.EventHandler(this.bRefreshPlaylists_Click);
            // 
            // dgvTracksOnList
            // 
            this.dgvTracksOnList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTracksOnList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.trTrackTitle,
            this.trArtist,
            this.trTrackID,
            this.dataGridViewTextBoxColumn4,
            this.trDuration});
            this.dgvTracksOnList.Location = new System.Drawing.Point(478, 33);
            this.dgvTracksOnList.Name = "dgvTracksOnList";
            this.dgvTracksOnList.Size = new System.Drawing.Size(414, 395);
            this.dgvTracksOnList.TabIndex = 4;
            // 
            // trTrackTitle
            // 
            this.trTrackTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.trTrackTitle.HeaderText = "Track Title";
            this.trTrackTitle.Name = "trTrackTitle";
            this.trTrackTitle.Width = 83;
            // 
            // trArtist
            // 
            this.trArtist.HeaderText = "Artist Name";
            this.trArtist.Name = "trArtist";
            // 
            // trTrackID
            // 
            this.trTrackID.HeaderText = "TrackID";
            this.trTrackID.Name = "trTrackID";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Uri";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // trDuration
            // 
            this.trDuration.HeaderText = "DurationMs";
            this.trDuration.Name = "trDuration";
            // 
            // bPlayAndRecord
            // 
            this.bPlayAndRecord.Location = new System.Drawing.Point(916, 33);
            this.bPlayAndRecord.Name = "bPlayAndRecord";
            this.bPlayAndRecord.Size = new System.Drawing.Size(119, 23);
            this.bPlayAndRecord.TabIndex = 5;
            this.bPlayAndRecord.Text = "Play and Record";
            this.bPlayAndRecord.UseVisualStyleBackColor = true;
            this.bPlayAndRecord.Click += new System.EventHandler(this.bPlayAndRecord_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(916, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Add Genres: ";
            // 
            // txtAddGenres
            // 
            this.txtAddGenres.Location = new System.Drawing.Point(995, 73);
            this.txtAddGenres.Name = "txtAddGenres";
            this.txtAddGenres.Size = new System.Drawing.Size(184, 20);
            this.txtAddGenres.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Start";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Finish";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bUKUpdate);
            this.groupBox1.Controls.Add(this.txtUKEnd);
            this.groupBox1.Controls.Add(this.txtUKStart);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(927, 307);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 158);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "UK Charts";
            // 
            // bUKUpdate
            // 
            this.bUKUpdate.Location = new System.Drawing.Point(32, 83);
            this.bUKUpdate.Name = "bUKUpdate";
            this.bUKUpdate.Size = new System.Drawing.Size(75, 23);
            this.bUKUpdate.TabIndex = 13;
            this.bUKUpdate.Text = "Update";
            this.bUKUpdate.UseVisualStyleBackColor = true;
            this.bUKUpdate.Click += new System.EventHandler(this.bUKUpdate_Click);
            // 
            // txtUKEnd
            // 
            this.txtUKEnd.Location = new System.Drawing.Point(95, 42);
            this.txtUKEnd.Name = "txtUKEnd";
            this.txtUKEnd.Size = new System.Drawing.Size(51, 20);
            this.txtUKEnd.TabIndex = 12;
            // 
            // txtUKStart
            // 
            this.txtUKStart.Location = new System.Drawing.Point(17, 42);
            this.txtUKStart.Name = "txtUKStart";
            this.txtUKStart.Size = new System.Drawing.Size(51, 20);
            this.txtUKStart.TabIndex = 11;
            // 
            // bTest
            // 
            this.bTest.Location = new System.Drawing.Point(919, 185);
            this.bTest.Name = "bTest";
            this.bTest.Size = new System.Drawing.Size(143, 23);
            this.bTest.TabIndex = 12;
            this.bTest.Text = "Check Token";
            this.bTest.UseVisualStyleBackColor = true;
            this.bTest.Click += new System.EventHandler(this.bTest_Click);
            // 
            // FullPlayList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 573);
            this.Controls.Add(this.bTest);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtAddGenres);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bPlayAndRecord);
            this.Controls.Add(this.dgvTracksOnList);
            this.Controls.Add(this.bRefreshPlaylists);
            this.Controls.Add(this.dgvPlaylists);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FullPlayList";
            this.Text = "Play and Record";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracksOnList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvPlaylists;
        private System.Windows.Forms.Button bRefreshPlaylists;
        private System.Windows.Forms.DataGridViewTextBoxColumn PLName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PLID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PLTracks;
        private System.Windows.Forms.DataGridViewTextBoxColumn PLUri;
        private System.Windows.Forms.DataGridView dgvTracksOnList;
        private System.Windows.Forms.Button bPlayAndRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn trTrackTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn trArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn trTrackID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn trDuration;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddGenres;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bUKUpdate;
        private System.Windows.Forms.TextBox txtUKEnd;
        private System.Windows.Forms.TextBox txtUKStart;
        private System.Windows.Forms.Button bTest;
    }
}