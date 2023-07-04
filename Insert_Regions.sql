USE [NZWalksDB]
GO

INSERT INTO [dbo].[Regions]
           ([Id]
           ,[Code]
           ,[Name]
           ,[RegionImageUrl])
     VALUES
           ('6e830467-f96b-4156-9d24-77bd5c68cb13','AKL','Auckland Region','https://cdn.pixabay.com/photo/2014/03/05/10/21/cornwall-park-279966_1280.jpg'),
		   ('9532b392-104b-4301-af18-2d2e50573b27','WLG','Wellington Region','https://www.live-work.immigration.govt.nz/sites/default/files/styles/scale_width_media_medium_/public/2020-06/Cable-car.jpg?itok=n2rcfKps');
GO