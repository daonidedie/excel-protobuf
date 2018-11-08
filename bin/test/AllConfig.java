// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: allConfig.proto

public final class AllConfig {
  private AllConfig() {}
  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistryLite registry) {
  }

  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistry registry) {
    registerAllExtensions(
        (com.google.protobuf.ExtensionRegistryLite) registry);
  }
  public interface allConfigOrBuilder extends
      // @@protoc_insertion_point(interface_extends:allConfig)
      com.google.protobuf.MessageOrBuilder {

    /**
     * <code>.t_propContainer propContainer = 1;</code>
     */
    boolean hasPropContainer();
    /**
     * <code>.t_propContainer propContainer = 1;</code>
     */
    TProp.t_propContainer getPropContainer();
    /**
     * <code>.t_propContainer propContainer = 1;</code>
     */
    TProp.t_propContainerOrBuilder getPropContainerOrBuilder();
  }
  /**
   * Protobuf type {@code allConfig}
   */
  public  static final class allConfig extends
      com.google.protobuf.GeneratedMessageV3 implements
      // @@protoc_insertion_point(message_implements:allConfig)
      allConfigOrBuilder {
  private static final long serialVersionUID = 0L;
    // Use allConfig.newBuilder() to construct.
    private allConfig(com.google.protobuf.GeneratedMessageV3.Builder<?> builder) {
      super(builder);
    }
    private allConfig() {
    }

    @java.lang.Override
    public final com.google.protobuf.UnknownFieldSet
    getUnknownFields() {
      return this.unknownFields;
    }
    private allConfig(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      this();
      if (extensionRegistry == null) {
        throw new java.lang.NullPointerException();
      }
      int mutable_bitField0_ = 0;
      com.google.protobuf.UnknownFieldSet.Builder unknownFields =
          com.google.protobuf.UnknownFieldSet.newBuilder();
      try {
        boolean done = false;
        while (!done) {
          int tag = input.readTag();
          switch (tag) {
            case 0:
              done = true;
              break;
            case 10: {
              TProp.t_propContainer.Builder subBuilder = null;
              if (propContainer_ != null) {
                subBuilder = propContainer_.toBuilder();
              }
              propContainer_ = input.readMessage(TProp.t_propContainer.parser(), extensionRegistry);
              if (subBuilder != null) {
                subBuilder.mergeFrom(propContainer_);
                propContainer_ = subBuilder.buildPartial();
              }

              break;
            }
            default: {
              if (!parseUnknownFieldProto3(
                  input, unknownFields, extensionRegistry, tag)) {
                done = true;
              }
              break;
            }
          }
        }
      } catch (com.google.protobuf.InvalidProtocolBufferException e) {
        throw e.setUnfinishedMessage(this);
      } catch (java.io.IOException e) {
        throw new com.google.protobuf.InvalidProtocolBufferException(
            e).setUnfinishedMessage(this);
      } finally {
        this.unknownFields = unknownFields.build();
        makeExtensionsImmutable();
      }
    }
    public static final com.google.protobuf.Descriptors.Descriptor
        getDescriptor() {
      return AllConfig.internal_static_allConfig_descriptor;
    }

    @java.lang.Override
    protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
        internalGetFieldAccessorTable() {
      return AllConfig.internal_static_allConfig_fieldAccessorTable
          .ensureFieldAccessorsInitialized(
              AllConfig.allConfig.class, AllConfig.allConfig.Builder.class);
    }

    public static final int PROPCONTAINER_FIELD_NUMBER = 1;
    private TProp.t_propContainer propContainer_;
    /**
     * <code>.t_propContainer propContainer = 1;</code>
     */
    public boolean hasPropContainer() {
      return propContainer_ != null;
    }
    /**
     * <code>.t_propContainer propContainer = 1;</code>
     */
    public TProp.t_propContainer getPropContainer() {
      return propContainer_ == null ? TProp.t_propContainer.getDefaultInstance() : propContainer_;
    }
    /**
     * <code>.t_propContainer propContainer = 1;</code>
     */
    public TProp.t_propContainerOrBuilder getPropContainerOrBuilder() {
      return getPropContainer();
    }

    private byte memoizedIsInitialized = -1;
    @java.lang.Override
    public final boolean isInitialized() {
      byte isInitialized = memoizedIsInitialized;
      if (isInitialized == 1) return true;
      if (isInitialized == 0) return false;

      memoizedIsInitialized = 1;
      return true;
    }

    @java.lang.Override
    public void writeTo(com.google.protobuf.CodedOutputStream output)
                        throws java.io.IOException {
      if (propContainer_ != null) {
        output.writeMessage(1, getPropContainer());
      }
      unknownFields.writeTo(output);
    }

    @java.lang.Override
    public int getSerializedSize() {
      int size = memoizedSize;
      if (size != -1) return size;

      size = 0;
      if (propContainer_ != null) {
        size += com.google.protobuf.CodedOutputStream
          .computeMessageSize(1, getPropContainer());
      }
      size += unknownFields.getSerializedSize();
      memoizedSize = size;
      return size;
    }

    @java.lang.Override
    public boolean equals(final java.lang.Object obj) {
      if (obj == this) {
       return true;
      }
      if (!(obj instanceof AllConfig.allConfig)) {
        return super.equals(obj);
      }
      AllConfig.allConfig other = (AllConfig.allConfig) obj;

      boolean result = true;
      result = result && (hasPropContainer() == other.hasPropContainer());
      if (hasPropContainer()) {
        result = result && getPropContainer()
            .equals(other.getPropContainer());
      }
      result = result && unknownFields.equals(other.unknownFields);
      return result;
    }

    @java.lang.Override
    public int hashCode() {
      if (memoizedHashCode != 0) {
        return memoizedHashCode;
      }
      int hash = 41;
      hash = (19 * hash) + getDescriptor().hashCode();
      if (hasPropContainer()) {
        hash = (37 * hash) + PROPCONTAINER_FIELD_NUMBER;
        hash = (53 * hash) + getPropContainer().hashCode();
      }
      hash = (29 * hash) + unknownFields.hashCode();
      memoizedHashCode = hash;
      return hash;
    }

    public static AllConfig.allConfig parseFrom(
        java.nio.ByteBuffer data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static AllConfig.allConfig parseFrom(
        java.nio.ByteBuffer data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static AllConfig.allConfig parseFrom(
        com.google.protobuf.ByteString data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static AllConfig.allConfig parseFrom(
        com.google.protobuf.ByteString data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static AllConfig.allConfig parseFrom(byte[] data)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data);
    }
    public static AllConfig.allConfig parseFrom(
        byte[] data,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return PARSER.parseFrom(data, extensionRegistry);
    }
    public static AllConfig.allConfig parseFrom(java.io.InputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input);
    }
    public static AllConfig.allConfig parseFrom(
        java.io.InputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input, extensionRegistry);
    }
    public static AllConfig.allConfig parseDelimitedFrom(java.io.InputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseDelimitedWithIOException(PARSER, input);
    }
    public static AllConfig.allConfig parseDelimitedFrom(
        java.io.InputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseDelimitedWithIOException(PARSER, input, extensionRegistry);
    }
    public static AllConfig.allConfig parseFrom(
        com.google.protobuf.CodedInputStream input)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input);
    }
    public static AllConfig.allConfig parseFrom(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      return com.google.protobuf.GeneratedMessageV3
          .parseWithIOException(PARSER, input, extensionRegistry);
    }

    @java.lang.Override
    public Builder newBuilderForType() { return newBuilder(); }
    public static Builder newBuilder() {
      return DEFAULT_INSTANCE.toBuilder();
    }
    public static Builder newBuilder(AllConfig.allConfig prototype) {
      return DEFAULT_INSTANCE.toBuilder().mergeFrom(prototype);
    }
    @java.lang.Override
    public Builder toBuilder() {
      return this == DEFAULT_INSTANCE
          ? new Builder() : new Builder().mergeFrom(this);
    }

    @java.lang.Override
    protected Builder newBuilderForType(
        com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
      Builder builder = new Builder(parent);
      return builder;
    }
    /**
     * Protobuf type {@code allConfig}
     */
    public static final class Builder extends
        com.google.protobuf.GeneratedMessageV3.Builder<Builder> implements
        // @@protoc_insertion_point(builder_implements:allConfig)
        AllConfig.allConfigOrBuilder {
      public static final com.google.protobuf.Descriptors.Descriptor
          getDescriptor() {
        return AllConfig.internal_static_allConfig_descriptor;
      }

      @java.lang.Override
      protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
          internalGetFieldAccessorTable() {
        return AllConfig.internal_static_allConfig_fieldAccessorTable
            .ensureFieldAccessorsInitialized(
                AllConfig.allConfig.class, AllConfig.allConfig.Builder.class);
      }

      // Construct using AllConfig.allConfig.newBuilder()
      private Builder() {
        maybeForceBuilderInitialization();
      }

      private Builder(
          com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
        super(parent);
        maybeForceBuilderInitialization();
      }
      private void maybeForceBuilderInitialization() {
        if (com.google.protobuf.GeneratedMessageV3
                .alwaysUseFieldBuilders) {
        }
      }
      @java.lang.Override
      public Builder clear() {
        super.clear();
        if (propContainerBuilder_ == null) {
          propContainer_ = null;
        } else {
          propContainer_ = null;
          propContainerBuilder_ = null;
        }
        return this;
      }

      @java.lang.Override
      public com.google.protobuf.Descriptors.Descriptor
          getDescriptorForType() {
        return AllConfig.internal_static_allConfig_descriptor;
      }

      @java.lang.Override
      public AllConfig.allConfig getDefaultInstanceForType() {
        return AllConfig.allConfig.getDefaultInstance();
      }

      @java.lang.Override
      public AllConfig.allConfig build() {
        AllConfig.allConfig result = buildPartial();
        if (!result.isInitialized()) {
          throw newUninitializedMessageException(result);
        }
        return result;
      }

      @java.lang.Override
      public AllConfig.allConfig buildPartial() {
        AllConfig.allConfig result = new AllConfig.allConfig(this);
        if (propContainerBuilder_ == null) {
          result.propContainer_ = propContainer_;
        } else {
          result.propContainer_ = propContainerBuilder_.build();
        }
        onBuilt();
        return result;
      }

      @java.lang.Override
      public Builder clone() {
        return (Builder) super.clone();
      }
      @java.lang.Override
      public Builder setField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          java.lang.Object value) {
        return (Builder) super.setField(field, value);
      }
      @java.lang.Override
      public Builder clearField(
          com.google.protobuf.Descriptors.FieldDescriptor field) {
        return (Builder) super.clearField(field);
      }
      @java.lang.Override
      public Builder clearOneof(
          com.google.protobuf.Descriptors.OneofDescriptor oneof) {
        return (Builder) super.clearOneof(oneof);
      }
      @java.lang.Override
      public Builder setRepeatedField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          int index, java.lang.Object value) {
        return (Builder) super.setRepeatedField(field, index, value);
      }
      @java.lang.Override
      public Builder addRepeatedField(
          com.google.protobuf.Descriptors.FieldDescriptor field,
          java.lang.Object value) {
        return (Builder) super.addRepeatedField(field, value);
      }
      @java.lang.Override
      public Builder mergeFrom(com.google.protobuf.Message other) {
        if (other instanceof AllConfig.allConfig) {
          return mergeFrom((AllConfig.allConfig)other);
        } else {
          super.mergeFrom(other);
          return this;
        }
      }

      public Builder mergeFrom(AllConfig.allConfig other) {
        if (other == AllConfig.allConfig.getDefaultInstance()) return this;
        if (other.hasPropContainer()) {
          mergePropContainer(other.getPropContainer());
        }
        this.mergeUnknownFields(other.unknownFields);
        onChanged();
        return this;
      }

      @java.lang.Override
      public final boolean isInitialized() {
        return true;
      }

      @java.lang.Override
      public Builder mergeFrom(
          com.google.protobuf.CodedInputStream input,
          com.google.protobuf.ExtensionRegistryLite extensionRegistry)
          throws java.io.IOException {
        AllConfig.allConfig parsedMessage = null;
        try {
          parsedMessage = PARSER.parsePartialFrom(input, extensionRegistry);
        } catch (com.google.protobuf.InvalidProtocolBufferException e) {
          parsedMessage = (AllConfig.allConfig) e.getUnfinishedMessage();
          throw e.unwrapIOException();
        } finally {
          if (parsedMessage != null) {
            mergeFrom(parsedMessage);
          }
        }
        return this;
      }

      private TProp.t_propContainer propContainer_ = null;
      private com.google.protobuf.SingleFieldBuilderV3<
          TProp.t_propContainer, TProp.t_propContainer.Builder, TProp.t_propContainerOrBuilder> propContainerBuilder_;
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public boolean hasPropContainer() {
        return propContainerBuilder_ != null || propContainer_ != null;
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public TProp.t_propContainer getPropContainer() {
        if (propContainerBuilder_ == null) {
          return propContainer_ == null ? TProp.t_propContainer.getDefaultInstance() : propContainer_;
        } else {
          return propContainerBuilder_.getMessage();
        }
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public Builder setPropContainer(TProp.t_propContainer value) {
        if (propContainerBuilder_ == null) {
          if (value == null) {
            throw new NullPointerException();
          }
          propContainer_ = value;
          onChanged();
        } else {
          propContainerBuilder_.setMessage(value);
        }

        return this;
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public Builder setPropContainer(
          TProp.t_propContainer.Builder builderForValue) {
        if (propContainerBuilder_ == null) {
          propContainer_ = builderForValue.build();
          onChanged();
        } else {
          propContainerBuilder_.setMessage(builderForValue.build());
        }

        return this;
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public Builder mergePropContainer(TProp.t_propContainer value) {
        if (propContainerBuilder_ == null) {
          if (propContainer_ != null) {
            propContainer_ =
              TProp.t_propContainer.newBuilder(propContainer_).mergeFrom(value).buildPartial();
          } else {
            propContainer_ = value;
          }
          onChanged();
        } else {
          propContainerBuilder_.mergeFrom(value);
        }

        return this;
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public Builder clearPropContainer() {
        if (propContainerBuilder_ == null) {
          propContainer_ = null;
          onChanged();
        } else {
          propContainer_ = null;
          propContainerBuilder_ = null;
        }

        return this;
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public TProp.t_propContainer.Builder getPropContainerBuilder() {
        
        onChanged();
        return getPropContainerFieldBuilder().getBuilder();
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      public TProp.t_propContainerOrBuilder getPropContainerOrBuilder() {
        if (propContainerBuilder_ != null) {
          return propContainerBuilder_.getMessageOrBuilder();
        } else {
          return propContainer_ == null ?
              TProp.t_propContainer.getDefaultInstance() : propContainer_;
        }
      }
      /**
       * <code>.t_propContainer propContainer = 1;</code>
       */
      private com.google.protobuf.SingleFieldBuilderV3<
          TProp.t_propContainer, TProp.t_propContainer.Builder, TProp.t_propContainerOrBuilder> 
          getPropContainerFieldBuilder() {
        if (propContainerBuilder_ == null) {
          propContainerBuilder_ = new com.google.protobuf.SingleFieldBuilderV3<
              TProp.t_propContainer, TProp.t_propContainer.Builder, TProp.t_propContainerOrBuilder>(
                  getPropContainer(),
                  getParentForChildren(),
                  isClean());
          propContainer_ = null;
        }
        return propContainerBuilder_;
      }
      @java.lang.Override
      public final Builder setUnknownFields(
          final com.google.protobuf.UnknownFieldSet unknownFields) {
        return super.setUnknownFieldsProto3(unknownFields);
      }

      @java.lang.Override
      public final Builder mergeUnknownFields(
          final com.google.protobuf.UnknownFieldSet unknownFields) {
        return super.mergeUnknownFields(unknownFields);
      }


      // @@protoc_insertion_point(builder_scope:allConfig)
    }

    // @@protoc_insertion_point(class_scope:allConfig)
    private static final AllConfig.allConfig DEFAULT_INSTANCE;
    static {
      DEFAULT_INSTANCE = new AllConfig.allConfig();
    }

    public static AllConfig.allConfig getDefaultInstance() {
      return DEFAULT_INSTANCE;
    }

    private static final com.google.protobuf.Parser<allConfig>
        PARSER = new com.google.protobuf.AbstractParser<allConfig>() {
      @java.lang.Override
      public allConfig parsePartialFrom(
          com.google.protobuf.CodedInputStream input,
          com.google.protobuf.ExtensionRegistryLite extensionRegistry)
          throws com.google.protobuf.InvalidProtocolBufferException {
        return new allConfig(input, extensionRegistry);
      }
    };

    public static com.google.protobuf.Parser<allConfig> parser() {
      return PARSER;
    }

    @java.lang.Override
    public com.google.protobuf.Parser<allConfig> getParserForType() {
      return PARSER;
    }

    @java.lang.Override
    public AllConfig.allConfig getDefaultInstanceForType() {
      return DEFAULT_INSTANCE;
    }

  }

  private static final com.google.protobuf.Descriptors.Descriptor
    internal_static_allConfig_descriptor;
  private static final 
    com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
      internal_static_allConfig_fieldAccessorTable;

  public static com.google.protobuf.Descriptors.FileDescriptor
      getDescriptor() {
    return descriptor;
  }
  private static  com.google.protobuf.Descriptors.FileDescriptor
      descriptor;
  static {
    java.lang.String[] descriptorData = {
      "\n\017allConfig.proto\032\014t_prop.proto\"4\n\tallCo" +
      "nfig\022\'\n\rpropContainer\030\001 \001(\0132\020.t_propCont" +
      "ainerb\006proto3"
    };
    com.google.protobuf.Descriptors.FileDescriptor.InternalDescriptorAssigner assigner =
        new com.google.protobuf.Descriptors.FileDescriptor.    InternalDescriptorAssigner() {
          public com.google.protobuf.ExtensionRegistry assignDescriptors(
              com.google.protobuf.Descriptors.FileDescriptor root) {
            descriptor = root;
            return null;
          }
        };
    com.google.protobuf.Descriptors.FileDescriptor
      .internalBuildGeneratedFileFrom(descriptorData,
        new com.google.protobuf.Descriptors.FileDescriptor[] {
          TProp.getDescriptor(),
        }, assigner);
    internal_static_allConfig_descriptor =
      getDescriptor().getMessageTypes().get(0);
    internal_static_allConfig_fieldAccessorTable = new
      com.google.protobuf.GeneratedMessageV3.FieldAccessorTable(
        internal_static_allConfig_descriptor,
        new java.lang.String[] { "PropContainer", });
    TProp.getDescriptor();
  }

  // @@protoc_insertion_point(outer_class_scope)
}
