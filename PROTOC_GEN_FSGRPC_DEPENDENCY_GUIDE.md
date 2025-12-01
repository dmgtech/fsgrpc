# How protoc-gen-fsgrpc is Configured in FsGrpc.Tests

## Current Configuration

### buf.gen.yaml Location
`FsGrpc/FsGrpc.Tests/buf.gen.yaml`

### Current Content
```yaml
version: v1
plugins:
  - name: fsgrpc
    path: ../../protoc-gen-fsgrpc/bin/Debug/net6.0/protoc-gen-fsgrpc
    out: gen
```

## The Issue: Path Mismatch

The buf.gen.yaml references:
```
../../protoc-gen-fsgrpc/bin/Debug/net6.0/protoc-gen-fsgrpc
```

But the actual repository is named:
```
FsGrpc.ProtocGenFsGrpc
```

## Repository Structure

```
C:\Users\gregory.allen\source\repos\
??? fsgrpc\                          ? You are here
?   ??? FsGrpc\
?       ??? FsGrpc.Tests\
?           ??? buf.gen.yaml         ? Configuration file
?           ??? struct_test.proto    ? Your new proto file
?           ??? testproto.proto
?           ??? gen\                 ? Generated code goes here
?
??? FsGrpc.ProtocGenFsGrpc\         ? Sibling repository
    ??? bin\Debug\net6.0\
        ??? protoc-gen-fsgrpc.exe    ? Code generator executable
```

## Dependency Type: **Local Path Reference**

This is **NOT** a NuGet package or transient dependency. It's a:
- ? **Direct local path reference** to the sibling repository
- ? **Development-time dependency** (not included in published packages)
- ? **Buf plugin configuration** (buf CLI uses this path to find the generator)

## What You Need to Do

### Step 1: Fix the Path in buf.gen.yaml

The path in buf.gen.yaml needs to be corrected to match the actual repository name:

**Current (Incorrect):**
```yaml
path: ../../protoc-gen-fsgrpc/bin/Debug/net6.0/protoc-gen-fsgrpc
```

**Should be:**
```yaml
path: ../../../FsGrpc.ProtocGenFsGrpc/bin/Debug/net6.0/protoc-gen-fsgrpc
```

Note: The relative path changes because:
- From `FsGrpc/FsGrpc.Tests/` to sibling repo requires going up 3 levels, not 2
- Current path: `repos/fsgrpc/FsGrpc/FsGrpc.Tests/`
- Target path: `repos/FsGrpc.ProtocGenFsGrpc/`

### Step 2: Build protoc-gen-fsgrpc

```powershell
cd C:\Users\gregory.allen\source\repos\FsGrpc.ProtocGenFsGrpc
dotnet build
```

This will create:
```
FsGrpc.ProtocGenFsGrpc/bin/Debug/net6.0/protoc-gen-fsgrpc.exe
```

### Step 3: Return to FsGrpc and Generate Code

```powershell
cd C:\Users\gregory.allen\source\repos\fsgrpc\FsGrpc\FsGrpc.Tests
buf generate --include-imports --include-wkt
```

### Step 4: Verify Generated Files

Check that these files were created:
```
gen/google/protobuf/struct.proto.gen.fs
gen/struct_test.proto.gen.fs
```

## Alternative: Use Symbolic Link

If you prefer to keep the path as `../../protoc-gen-fsgrpc`, you could create a symbolic link:

```powershell
cd C:\Users\gregory.allen\source\repos
New-Item -ItemType SymbolicLink -Path "protoc-gen-fsgrpc" -Target "FsGrpc.ProtocGenFsGrpc"
```

But it's cleaner to just fix the path in buf.gen.yaml.

## Why This Configuration?

This setup allows:
1. **Local Development**: You can modify the generator and immediately test changes
2. **No NuGet Publishing Required**: For testing, you don't need to publish to NuGet
3. **Consistent Build Process**: Same buf generate command works for everyone
4. **CI/CD Friendly**: In CI, you can build protoc-gen-fsgrpc first, then run tests

## What About Production Use?

For end users (not developers):
- They use **FsGrpc.Tools** NuGet package
- FsGrpc.Tools contains the protoc-gen-fsgrpc executable
- MSBuild integration handles code generation automatically
- No manual buf generate needed

## Summary

| Question | Answer |
|----------|--------|
| How is dependency defined? | Local path in `buf.gen.yaml` |
| Is it in NuGet? | No, it's a local development reference |
| Where is the executable? | `FsGrpc.ProtocGenFsGrpc/bin/Debug/net6.0/` |
| What needs to be fixed? | Update path in buf.gen.yaml |
| Is this transient? | No, it's a direct path reference |
| What do end users use? | FsGrpc.Tools NuGet package (different) |

## Next Steps

1. ? Fix path in `buf.gen.yaml`
2. ? Build `FsGrpc.ProtocGenFsGrpc`
3. ? Run `buf generate --include-imports --include-wkt`
4. ? Verify generated files exist
5. ? Build and test your changes

## Updated buf.gen.yaml

Here's what your buf.gen.yaml should contain:

```yaml
version: v1
plugins:
  - name: fsgrpc
    path: ../../../FsGrpc.ProtocGenFsGrpc/bin/Debug/net6.0/protoc-gen-fsgrpc
    out: gen
```

Or if the repository is actually at a different relative location, adjust accordingly. The key is matching the actual filesystem structure.
