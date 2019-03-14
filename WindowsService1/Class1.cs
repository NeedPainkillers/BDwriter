using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    class PostTextEqualityComparer : IEqualityComparer<PostText>
    {
        public bool Equals(PostText b1, PostText b2)
        {
            return b1.idPost.Equals(b2.idPost);
        }

        public int GetHashCode(PostText bx)
        {
            return bx.idPost.GetHashCode();
        }
    }
    class PostLinkEqualityComparer : IEqualityComparer<PostLink>
    {
        public bool Equals(PostLink b1, PostLink b2)
        {
            return b1.idPost.Equals(b2.idPost);
        }

        public int GetHashCode(PostLink bx)
        {
            return bx.idPost.GetHashCode();
        }
    }
    class PostImgLinkEqualityComparer : IEqualityComparer<PostImgLink>
    {
        public bool Equals(PostImgLink b1, PostImgLink b2)
        {
            return b1.idPost.Equals(b2.idPost);
        }

        public int GetHashCode(PostImgLink bx)
        {
            return bx.idPost.GetHashCode();
        }
    }
}
