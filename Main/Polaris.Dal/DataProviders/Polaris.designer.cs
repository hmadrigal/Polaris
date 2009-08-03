﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3074
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Polaris.Dal
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="Polaris.Dbl")]
	public partial class SourceDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertContentType(ContentType instance);
    partial void UpdateContentType(ContentType instance);
    partial void DeleteContentType(ContentType instance);
    partial void InsertDeveloper(Developer instance);
    partial void UpdateDeveloper(Developer instance);
    partial void DeleteDeveloper(Developer instance);
    partial void InsertDevelopmentTeam(DevelopmentTeam instance);
    partial void UpdateDevelopmentTeam(DevelopmentTeam instance);
    partial void DeleteDevelopmentTeam(DevelopmentTeam instance);
    partial void InsertFilterOption(FilterOption instance);
    partial void UpdateFilterOption(FilterOption instance);
    partial void DeleteFilterOption(FilterOption instance);
    partial void InsertGame(Game instance);
    partial void UpdateGame(Game instance);
    partial void DeleteGame(Game instance);
    partial void InsertPlayLog(PlayLog instance);
    partial void UpdatePlayLog(PlayLog instance);
    partial void DeletePlayLog(PlayLog instance);
    partial void InsertSortOption(SortOption instance);
    partial void UpdateSortOption(SortOption instance);
    partial void DeleteSortOption(SortOption instance);
    partial void InsertTimePeriod(TimePeriod instance);
    partial void UpdateTimePeriod(TimePeriod instance);
    partial void DeleteTimePeriod(TimePeriod instance);
    partial void InsertMenuItem(MenuItem instance);
    partial void UpdateMenuItem(MenuItem instance);
    partial void DeleteMenuItem(MenuItem instance);
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    #endregion
		
		public SourceDataContext() : 
				base(global::Polaris.Dal.Properties.Settings.Default.PolarisConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public SourceDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SourceDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SourceDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SourceDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ContentType> ContentTypes
		{
			get
			{
				return this.GetTable<ContentType>();
			}
		}
		
		public System.Data.Linq.Table<Developer> Developers
		{
			get
			{
				return this.GetTable<Developer>();
			}
		}
		
		public System.Data.Linq.Table<DevelopmentTeam> DevelopmentTeams
		{
			get
			{
				return this.GetTable<DevelopmentTeam>();
			}
		}
		
		public System.Data.Linq.Table<FilterOption> FilterOptions
		{
			get
			{
				return this.GetTable<FilterOption>();
			}
		}
		
		public System.Data.Linq.Table<Game> Games
		{
			get
			{
				return this.GetTable<Game>();
			}
		}
		
		public System.Data.Linq.Table<PlayLog> PlayLogs
		{
			get
			{
				return this.GetTable<PlayLog>();
			}
		}
		
		public System.Data.Linq.Table<SortOption> SortOptions
		{
			get
			{
				return this.GetTable<SortOption>();
			}
		}
		
		public System.Data.Linq.Table<TimePeriod> TimePeriods
		{
			get
			{
				return this.GetTable<TimePeriod>();
			}
		}
		
		public System.Data.Linq.Table<MenuItem> MenuItems
		{
			get
			{
				return this.GetTable<MenuItem>();
			}
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
	}
	
	[Table(Name="Polaris.ContentType")]
	public partial class ContentType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ContentTypeId;
		
		private string _Name;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnContentTypeIdChanging(int value);
    partial void OnContentTypeIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public ContentType()
		{
			OnCreated();
		}
		
		[Column(Storage="_ContentTypeId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ContentTypeId
		{
			get
			{
				return this._ContentTypeId;
			}
			set
			{
				if ((this._ContentTypeId != value))
				{
					this.OnContentTypeIdChanging(value);
					this.SendPropertyChanging();
					this._ContentTypeId = value;
					this.SendPropertyChanged("ContentTypeId");
					this.OnContentTypeIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.Developer")]
	public partial class Developer : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _DeveloperId;
		
		private long _UserId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDeveloperIdChanging(long value);
    partial void OnDeveloperIdChanged();
    partial void OnUserIdChanging(long value);
    partial void OnUserIdChanged();
    #endregion
		
		public Developer()
		{
			OnCreated();
		}
		
		[Column(Storage="_DeveloperId", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long DeveloperId
		{
			get
			{
				return this._DeveloperId;
			}
			set
			{
				if ((this._DeveloperId != value))
				{
					this.OnDeveloperIdChanging(value);
					this.SendPropertyChanging();
					this._DeveloperId = value;
					this.SendPropertyChanged("DeveloperId");
					this.OnDeveloperIdChanged();
				}
			}
		}
		
		[Column(Storage="_UserId", DbType="BigInt NOT NULL")]
		public long UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.DevelopmentTeam")]
	public partial class DevelopmentTeam : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _DevelopmentTeamId;
		
		private string _Name;
		
		private System.Guid _Key;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDevelopmentTeamIdChanging(long value);
    partial void OnDevelopmentTeamIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnKeyChanging(System.Guid value);
    partial void OnKeyChanged();
    #endregion
		
		public DevelopmentTeam()
		{
			OnCreated();
		}
		
		[Column(Storage="_DevelopmentTeamId", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long DevelopmentTeamId
		{
			get
			{
				return this._DevelopmentTeamId;
			}
			set
			{
				if ((this._DevelopmentTeamId != value))
				{
					this.OnDevelopmentTeamIdChanging(value);
					this.SendPropertyChanging();
					this._DevelopmentTeamId = value;
					this.SendPropertyChanged("DevelopmentTeamId");
					this.OnDevelopmentTeamIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(MAX) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Name="[Key]", Storage="_Key", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				if ((this._Key != value))
				{
					this.OnKeyChanging(value);
					this.SendPropertyChanging();
					this._Key = value;
					this.SendPropertyChanged("Key");
					this.OnKeyChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.FilterOption")]
	public partial class FilterOption : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FilterOptionId;
		
		private string _ColumnName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFilterOptionIdChanging(int value);
    partial void OnFilterOptionIdChanged();
    partial void OnColumnNameChanging(string value);
    partial void OnColumnNameChanged();
    #endregion
		
		public FilterOption()
		{
			OnCreated();
		}
		
		[Column(Storage="_FilterOptionId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int FilterOptionId
		{
			get
			{
				return this._FilterOptionId;
			}
			set
			{
				if ((this._FilterOptionId != value))
				{
					this.OnFilterOptionIdChanging(value);
					this.SendPropertyChanging();
					this._FilterOptionId = value;
					this.SendPropertyChanged("FilterOptionId");
					this.OnFilterOptionIdChanged();
				}
			}
		}
		
		[Column(Storage="_ColumnName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ColumnName
		{
			get
			{
				return this._ColumnName;
			}
			set
			{
				if ((this._ColumnName != value))
				{
					this.OnColumnNameChanging(value);
					this.SendPropertyChanging();
					this._ColumnName = value;
					this.SendPropertyChanged("ColumnName");
					this.OnColumnNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.Game")]
	public partial class Game : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _GameId;
		
		private string _Name;
		
		private System.Guid _Key;
		
		private bool _Active;
		
		private long _DevelopmentTeamId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnGameIdChanging(long value);
    partial void OnGameIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnKeyChanging(System.Guid value);
    partial void OnKeyChanged();
    partial void OnActiveChanging(bool value);
    partial void OnActiveChanged();
    partial void OnDevelopmentTeamIdChanging(long value);
    partial void OnDevelopmentTeamIdChanged();
    #endregion
		
		public Game()
		{
			OnCreated();
		}
		
		[Column(Storage="_GameId", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long GameId
		{
			get
			{
				return this._GameId;
			}
			set
			{
				if ((this._GameId != value))
				{
					this.OnGameIdChanging(value);
					this.SendPropertyChanging();
					this._GameId = value;
					this.SendPropertyChanged("GameId");
					this.OnGameIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(30) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Name="[Key]", Storage="_Key", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				if ((this._Key != value))
				{
					this.OnKeyChanging(value);
					this.SendPropertyChanging();
					this._Key = value;
					this.SendPropertyChanged("Key");
					this.OnKeyChanged();
				}
			}
		}
		
		[Column(Storage="_Active", DbType="Bit NOT NULL")]
		public bool Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this.OnActiveChanging(value);
					this.SendPropertyChanging();
					this._Active = value;
					this.SendPropertyChanged("Active");
					this.OnActiveChanged();
				}
			}
		}
		
		[Column(Storage="_DevelopmentTeamId", DbType="BigInt NOT NULL")]
		public long DevelopmentTeamId
		{
			get
			{
				return this._DevelopmentTeamId;
			}
			set
			{
				if ((this._DevelopmentTeamId != value))
				{
					this.OnDevelopmentTeamIdChanging(value);
					this.SendPropertyChanging();
					this._DevelopmentTeamId = value;
					this.SendPropertyChanged("DevelopmentTeamId");
					this.OnDevelopmentTeamIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.PlayLog")]
	public partial class PlayLog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _PlayLogId;
		
		private long _GameId;
		
		private long _UserId;
		
		private System.DateTime _Date;
		
		private decimal _Score;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPlayLogIdChanging(long value);
    partial void OnPlayLogIdChanged();
    partial void OnGameIdChanging(long value);
    partial void OnGameIdChanged();
    partial void OnUserIdChanging(long value);
    partial void OnUserIdChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    partial void OnScoreChanging(decimal value);
    partial void OnScoreChanged();
    #endregion
		
		public PlayLog()
		{
			OnCreated();
		}
		
		[Column(Storage="_PlayLogId", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long PlayLogId
		{
			get
			{
				return this._PlayLogId;
			}
			set
			{
				if ((this._PlayLogId != value))
				{
					this.OnPlayLogIdChanging(value);
					this.SendPropertyChanging();
					this._PlayLogId = value;
					this.SendPropertyChanged("PlayLogId");
					this.OnPlayLogIdChanged();
				}
			}
		}
		
		[Column(Storage="_GameId", DbType="BigInt NOT NULL")]
		public long GameId
		{
			get
			{
				return this._GameId;
			}
			set
			{
				if ((this._GameId != value))
				{
					this.OnGameIdChanging(value);
					this.SendPropertyChanging();
					this._GameId = value;
					this.SendPropertyChanged("GameId");
					this.OnGameIdChanged();
				}
			}
		}
		
		[Column(Storage="_UserId", DbType="BigInt NOT NULL")]
		public long UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[Column(Storage="_Date", DbType="DateTime NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[Column(Storage="_Score", DbType="Decimal(18,0) NOT NULL")]
		public decimal Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				if ((this._Score != value))
				{
					this.OnScoreChanging(value);
					this.SendPropertyChanging();
					this._Score = value;
					this.SendPropertyChanged("Score");
					this.OnScoreChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.SortOption")]
	public partial class SortOption : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _SortOptionId;
		
		private bool _Descending;
		
		private string _ColumnName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSortOptionIdChanging(int value);
    partial void OnSortOptionIdChanged();
    partial void OnDescendingChanging(bool value);
    partial void OnDescendingChanged();
    partial void OnColumnNameChanging(string value);
    partial void OnColumnNameChanged();
    #endregion
		
		public SortOption()
		{
			OnCreated();
		}
		
		[Column(Storage="_SortOptionId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int SortOptionId
		{
			get
			{
				return this._SortOptionId;
			}
			set
			{
				if ((this._SortOptionId != value))
				{
					this.OnSortOptionIdChanging(value);
					this.SendPropertyChanging();
					this._SortOptionId = value;
					this.SendPropertyChanged("SortOptionId");
					this.OnSortOptionIdChanged();
				}
			}
		}
		
		[Column(Storage="_Descending", DbType="Bit NOT NULL")]
		public bool Descending
		{
			get
			{
				return this._Descending;
			}
			set
			{
				if ((this._Descending != value))
				{
					this.OnDescendingChanging(value);
					this.SendPropertyChanging();
					this._Descending = value;
					this.SendPropertyChanged("Descending");
					this.OnDescendingChanged();
				}
			}
		}
		
		[Column(Storage="_ColumnName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ColumnName
		{
			get
			{
				return this._ColumnName;
			}
			set
			{
				if ((this._ColumnName != value))
				{
					this.OnColumnNameChanging(value);
					this.SendPropertyChanging();
					this._ColumnName = value;
					this.SendPropertyChanged("ColumnName");
					this.OnColumnNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.TimePeriod")]
	public partial class TimePeriod : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _TimePeriodId;
		
		private string _Name;
		
		private int _Hours;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnTimePeriodIdChanging(int value);
    partial void OnTimePeriodIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnHoursChanging(int value);
    partial void OnHoursChanged();
    #endregion
		
		public TimePeriod()
		{
			OnCreated();
		}
		
		[Column(Storage="_TimePeriodId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int TimePeriodId
		{
			get
			{
				return this._TimePeriodId;
			}
			set
			{
				if ((this._TimePeriodId != value))
				{
					this.OnTimePeriodIdChanging(value);
					this.SendPropertyChanging();
					this._TimePeriodId = value;
					this.SendPropertyChanged("TimePeriodId");
					this.OnTimePeriodIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_Hours", DbType="Int NOT NULL")]
		public int Hours
		{
			get
			{
				return this._Hours;
			}
			set
			{
				if ((this._Hours != value))
				{
					this.OnHoursChanging(value);
					this.SendPropertyChanging();
					this._Hours = value;
					this.SendPropertyChanged("Hours");
					this.OnHoursChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.MenuItem")]
	public partial class MenuItem : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _MenuItemId;
		
		private string _Name;
		
		private string _DisplayName;
		
		private int _SiteSection;
		
		private System.Nullable<int> _ParentMenuItemId;
		
		private System.Nullable<int> _ContentTypeId;
		
		private System.Nullable<int> _SortOptionId;
		
		private System.Nullable<int> _FilterOptionId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMenuItemIdChanging(int value);
    partial void OnMenuItemIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDisplayNameChanging(string value);
    partial void OnDisplayNameChanged();
    partial void OnSiteSectionChanging(int value);
    partial void OnSiteSectionChanged();
    partial void OnParentMenuItemIdChanging(System.Nullable<int> value);
    partial void OnParentMenuItemIdChanged();
    partial void OnContentTypeIdChanging(System.Nullable<int> value);
    partial void OnContentTypeIdChanged();
    partial void OnSortOptionIdChanging(System.Nullable<int> value);
    partial void OnSortOptionIdChanged();
    partial void OnFilterOptionIdChanging(System.Nullable<int> value);
    partial void OnFilterOptionIdChanged();
    #endregion
		
		public MenuItem()
		{
			OnCreated();
		}
		
		[Column(Storage="_MenuItemId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int MenuItemId
		{
			get
			{
				return this._MenuItemId;
			}
			set
			{
				if ((this._MenuItemId != value))
				{
					this.OnMenuItemIdChanging(value);
					this.SendPropertyChanging();
					this._MenuItemId = value;
					this.SendPropertyChanged("MenuItemId");
					this.OnMenuItemIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_DisplayName", DbType="VarChar(50)")]
		public string DisplayName
		{
			get
			{
				return this._DisplayName;
			}
			set
			{
				if ((this._DisplayName != value))
				{
					this.OnDisplayNameChanging(value);
					this.SendPropertyChanging();
					this._DisplayName = value;
					this.SendPropertyChanged("DisplayName");
					this.OnDisplayNameChanged();
				}
			}
		}
		
		[Column(Storage="_SiteSection", DbType="Int NOT NULL")]
		public int SiteSection
		{
			get
			{
				return this._SiteSection;
			}
			set
			{
				if ((this._SiteSection != value))
				{
					this.OnSiteSectionChanging(value);
					this.SendPropertyChanging();
					this._SiteSection = value;
					this.SendPropertyChanged("SiteSection");
					this.OnSiteSectionChanged();
				}
			}
		}
		
		[Column(Storage="_ParentMenuItemId", DbType="Int")]
		public System.Nullable<int> ParentMenuItemId
		{
			get
			{
				return this._ParentMenuItemId;
			}
			set
			{
				if ((this._ParentMenuItemId != value))
				{
					this.OnParentMenuItemIdChanging(value);
					this.SendPropertyChanging();
					this._ParentMenuItemId = value;
					this.SendPropertyChanged("ParentMenuItemId");
					this.OnParentMenuItemIdChanged();
				}
			}
		}
		
		[Column(Storage="_ContentTypeId", DbType="Int")]
		public System.Nullable<int> ContentTypeId
		{
			get
			{
				return this._ContentTypeId;
			}
			set
			{
				if ((this._ContentTypeId != value))
				{
					this.OnContentTypeIdChanging(value);
					this.SendPropertyChanging();
					this._ContentTypeId = value;
					this.SendPropertyChanged("ContentTypeId");
					this.OnContentTypeIdChanged();
				}
			}
		}
		
		[Column(Storage="_SortOptionId", DbType="Int")]
		public System.Nullable<int> SortOptionId
		{
			get
			{
				return this._SortOptionId;
			}
			set
			{
				if ((this._SortOptionId != value))
				{
					this.OnSortOptionIdChanging(value);
					this.SendPropertyChanging();
					this._SortOptionId = value;
					this.SendPropertyChanged("SortOptionId");
					this.OnSortOptionIdChanged();
				}
			}
		}
		
		[Column(Storage="_FilterOptionId", DbType="Int")]
		public System.Nullable<int> FilterOptionId
		{
			get
			{
				return this._FilterOptionId;
			}
			set
			{
				if ((this._FilterOptionId != value))
				{
					this.OnFilterOptionIdChanging(value);
					this.SendPropertyChanging();
					this._FilterOptionId = value;
					this.SendPropertyChanged("FilterOptionId");
					this.OnFilterOptionIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="Polaris.[User]")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _UserId;
		
		private string _Username;
		
		private string _Password;
		
		private string _FirstName;
		
		private string _LastName;
		
		private string _Email;
		
		private long _PlayCredits;
		
		private long _RankingCredits;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUserIdChanging(long value);
    partial void OnUserIdChanged();
    partial void OnUsernameChanging(string value);
    partial void OnUsernameChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnFirstNameChanging(string value);
    partial void OnFirstNameChanged();
    partial void OnLastNameChanging(string value);
    partial void OnLastNameChanged();
    partial void OnEmailChanging(string value);
    partial void OnEmailChanged();
    partial void OnPlayCreditsChanging(long value);
    partial void OnPlayCreditsChanged();
    partial void OnRankingCreditsChanging(long value);
    partial void OnRankingCreditsChanged();
    #endregion
		
		public User()
		{
			OnCreated();
		}
		
		[Column(Storage="_UserId", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[Column(Storage="_Username", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string Username
		{
			get
			{
				return this._Username;
			}
			set
			{
				if ((this._Username != value))
				{
					this.OnUsernameChanging(value);
					this.SendPropertyChanging();
					this._Username = value;
					this.SendPropertyChanged("Username");
					this.OnUsernameChanged();
				}
			}
		}
		
		[Column(Storage="_Password", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[Column(Storage="_FirstName", DbType="VarChar(30) NOT NULL", CanBeNull=false)]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				if ((this._FirstName != value))
				{
					this.OnFirstNameChanging(value);
					this.SendPropertyChanging();
					this._FirstName = value;
					this.SendPropertyChanged("FirstName");
					this.OnFirstNameChanged();
				}
			}
		}
		
		[Column(Storage="_LastName", DbType="VarChar(30) NOT NULL", CanBeNull=false)]
		public string LastName
		{
			get
			{
				return this._LastName;
			}
			set
			{
				if ((this._LastName != value))
				{
					this.OnLastNameChanging(value);
					this.SendPropertyChanging();
					this._LastName = value;
					this.SendPropertyChanged("LastName");
					this.OnLastNameChanged();
				}
			}
		}
		
		[Column(Storage="_Email", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}
			}
		}
		
		[Column(Storage="_PlayCredits", DbType="BigInt NOT NULL")]
		public long PlayCredits
		{
			get
			{
				return this._PlayCredits;
			}
			set
			{
				if ((this._PlayCredits != value))
				{
					this.OnPlayCreditsChanging(value);
					this.SendPropertyChanging();
					this._PlayCredits = value;
					this.SendPropertyChanged("PlayCredits");
					this.OnPlayCreditsChanged();
				}
			}
		}
		
		[Column(Storage="_RankingCredits", DbType="BigInt NOT NULL")]
		public long RankingCredits
		{
			get
			{
				return this._RankingCredits;
			}
			set
			{
				if ((this._RankingCredits != value))
				{
					this.OnRankingCreditsChanging(value);
					this.SendPropertyChanging();
					this._RankingCredits = value;
					this.SendPropertyChanged("RankingCredits");
					this.OnRankingCreditsChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
