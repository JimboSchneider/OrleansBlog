-- SQLite Orleans Persistence tables
-- Based on https://github.com/dotnet/orleans/blob/main/src/AdoNet/Shared/SQLite-Persistence.sql

CREATE TABLE IF NOT EXISTS OrleansStorage
(
    GrainIdHash INTEGER NOT NULL,
    GrainIdN0 INTEGER NOT NULL,
    GrainIdN1 INTEGER NOT NULL,
    GrainTypeHash INTEGER NOT NULL,
    GrainTypeString TEXT NOT NULL,
    GrainIdExtensionString TEXT NULL,
    ServiceId TEXT NOT NULL,
    PayloadBinary BLOB NULL,
    PayloadJson TEXT NULL,
    PayloadXml TEXT NULL,
    ModifiedOn TEXT NOT NULL,
    Version INTEGER NOT NULL DEFAULT 0,

    CONSTRAINT PK_Storage PRIMARY KEY (GrainIdHash, GrainTypeHash, GrainIdN0, GrainIdN1, GrainTypeString, GrainIdExtensionString, ServiceId)
);

CREATE INDEX IF NOT EXISTS IX_Storage ON OrleansStorage(GrainIdHash, GrainTypeHash, GrainIdN0, GrainIdN1, GrainTypeString, GrainIdExtensionString, ServiceId);