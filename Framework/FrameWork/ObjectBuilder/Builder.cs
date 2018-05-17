using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace DrectSoft.FrameWork.ObjectBuilder {
    /// <summary>
    /// DrectSoft 对象创建器
    /// </summary>
    public class Builder : BuilderBase<BuilderStage> {

        /// <summary>
        /// ctor
        /// </summary>
        public Builder() {
            Strategies.AddNew<TypeMappingStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<CreationStrategy>(BuilderStage.Creation);

            Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
        }
    }
}